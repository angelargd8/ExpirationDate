using UnityEngine;

public class EnemyRangedIA : MonoBehaviour
{
    private enum EnemyState
    {
        Patrol,
        KeepDistance,
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

    [Header("Distance Behavior")]
    [SerializeField] private float moveSpeed = 3.5f;

    // si el jugador mas cerca que esto que el enemigo huye
    [SerializeField] private float fleeDistance = 5f;

    // si el jugador esta entre fleeDistance y attackRange ataca
    [SerializeField] private float attackRange = 10f;

    // Si el jugador se aleja mas que esto, vuelve a patrullar
    [SerializeField] private float loseDistance = 16f;

    [Header("Vision")]
    [SerializeField] private float viewRadius = 12f;
    [SerializeField] private float viewAngle = 180f;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.25f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Animation")]
    [SerializeField] private Animator burgerAnimator;

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
                    currentState = EnemyState.KeepDistance;
                }

                break;

            case EnemyState.KeepDistance:
                if (!canSeePlayer || distanceToPlayer > loseDistance)
                {
                    ReturnToPatrol();
                    return;
                }

                if (distanceToPlayer <= fleeDistance)
                {
                    FleeFromPlayer();
                    TryAttack();
                    return;
                }

                if (distanceToPlayer <= attackRange)
                {
                    currentState = EnemyState.Attack;
                    return;
                }

                MoveTowards(player.position, moveSpeed);
                break;

            case EnemyState.Attack:
                if (!canSeePlayer || distanceToPlayer > loseDistance)
                {
                    ReturnToPatrol();
                    return;
                }

                if (distanceToPlayer <= fleeDistance)
                {
                    currentState = EnemyState.KeepDistance;
                    return;
                }

                if (distanceToPlayer > attackRange)
                {
                    currentState = EnemyState.KeepDistance;
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

    private void FleeFromPlayer()
    {
        Vector3 directionAway = transform.position - player.position;
        directionAway.y = 0f;

        if (directionAway.magnitude < 0.1f)
        {
            directionAway = -transform.forward;
        }

        Vector3 targetPosition = transform.position + directionAway.normalized * 3f;

        MoveTowards(targetPosition, moveSpeed);
        LookAtPlayer();
    }

    private void AttackPlayer()
    {
        StopHorizontalMovement();
        LookAtPlayer();
        TryAttack();
    }

    private void TryAttack()
    {
        if (ingredientThrower == null) return;

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

        if (burgerAnimator != null)
        {
            burgerAnimator.SetTrigger("Jump");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, fleeDistance);

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