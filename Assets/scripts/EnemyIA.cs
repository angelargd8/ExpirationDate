using UnityEngine;

public class EnemyIA : MonoBehaviour
{
    private enum EnemyState
    {
        Patrol,
        Chase,
        Attack
    }

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private IngredientThrower ingredientThrower;

    [Header("Patrol")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float waypointDistance = 0.5f;
    [SerializeField] private float waitTimeAtWaypoint = 1f;

    [Header("Chase")]
    [SerializeField] private float chaseSpeed = 3.5f;
    [SerializeField] private float loseDistance = 15f;

    [Header("Vision")]
    [SerializeField] private float viewRadius = 10f;
    [SerializeField] private float viewAngle = 120f;

    [Header("Attack")]
    [SerializeField] private float attackRange = 6f;
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.25f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;
    private EnemyState currentState = EnemyState.Patrol;

    private int currentWaypointIndex;
    private float lastAttackTime;
    private float waitTimer;
    private bool isWaiting;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (ingredientThrower == null)
        {
            ingredientThrower = GetComponent<IngredientThrower>();
        }
    }

    private void Update()
    {
        if (player == null)
        {
            StopHorizontalMovement();
            return;
        }

        CheckGround();

        bool canSeePlayer = LookForPlayer();
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();

                if (canSeePlayer)
                {
                    currentState = EnemyState.Chase;
                }

                break;

            case EnemyState.Chase:
                if (!canSeePlayer || distanceToPlayer > loseDistance)
                {
                    ReturnToPatrol();
                    return;
                }

                if (distanceToPlayer <= attackRange)
                {
                    currentState = EnemyState.Attack;
                    return;
                }

                ChasePlayer();
                break;

            case EnemyState.Attack:
                if (!canSeePlayer || distanceToPlayer > loseDistance)
                {
                    ReturnToPatrol();
                    return;
                }

                if (distanceToPlayer > attackRange)
                {
                    currentState = EnemyState.Chase;
                    return;
                }

                AttackPlayer();
                break;
        }
    }

    private void Patrol()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            StopHorizontalMovement();
            return;
        }

        if (isWaiting)
        {
            StopHorizontalMovement();

            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTimeAtWaypoint)
            {
                isWaiting = false;
                waitTimer = 0f;
                GoToNextWaypoint();
            }

            return;
        }

        Transform targetWaypoint = waypoints[currentWaypointIndex];

        MoveTowards(targetWaypoint.position, patrolSpeed);

        float distanceToWaypoint = Vector3.Distance(transform.position, targetWaypoint.position);

        if (distanceToWaypoint <= waypointDistance)
        {
            isWaiting = true;
            waitTimer = 0f;
        }
    }

    private void GoToNextWaypoint()
    {
        currentWaypointIndex++;

        if (currentWaypointIndex >= waypoints.Length)
        {
            currentWaypointIndex = 0;
        }
    }

    private void ChasePlayer()
    {
        MoveTowards(player.position, chaseSpeed);
    }

    private void AttackPlayer()
    {
        StopHorizontalMovement();
        LookAtPlayer();

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            ingredientThrower.ThrowIngredientTowards(player.position);
            lastAttackTime = Time.time;
        }
    }

    private void ReturnToPatrol()
    {
        currentState = EnemyState.Patrol;
        isWaiting = false;
        waitTimer = 0f;
    }

    private void MoveTowards(Vector3 targetPosition, float speed)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;

        if (direction.magnitude < 0.1f)
        {
            StopHorizontalMovement();
            return;
        }

        direction.Normalize();

        Vector3 horizontalVelocity = direction * speed;

        rb.linearVelocity = new Vector3(
            horizontalVelocity.x,
            rb.linearVelocity.y,
            horizontalVelocity.z
        );

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * 8f
        );
    }

    private void StopHorizontalMovement()
    {
        rb.linearVelocity = new Vector3(
            0f,
            rb.linearVelocity.y,
            0f
        );
    }

    private bool LookForPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;

        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > viewRadius)
        {
            return false;
        }

        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer.normalized);

        if (angleToPlayer > viewAngle * 0.5f)
        {
            return false;
        }

        return true;
    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.magnitude < 0.1f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * 10f
        );
    }

    private void CheckGround()
    {
        if (groundCheck == null)
        {
            isGrounded = false;
            return;
        }

        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    public void Jump()
    {
        if (!isGrounded) return;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.cyan;

        Vector3 leftLimit = Quaternion.Euler(0f, -viewAngle * 0.5f, 0f) * transform.forward;
        Vector3 rightLimit = Quaternion.Euler(0f, viewAngle * 0.5f, 0f) * transform.forward;

        Gizmos.DrawRay(transform.position, leftLimit * viewRadius);
        Gizmos.DrawRay(transform.position, rightLimit * viewRadius);

        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}