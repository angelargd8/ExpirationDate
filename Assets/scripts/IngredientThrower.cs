using UnityEngine;
using UnityEngine.InputSystem;

public class IngredientThrower : MonoBehaviour
{
    [Header("Ingredient")]
    [SerializeField] private IngredientData currentIngredient;

    [Header("Throw Settings")]
    [SerializeField] private Transform throwPoint;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float aimDistance = 50f;
    [SerializeField] private LayerMask aimLayerMask = ~0;

    [Header("Owner Throw Settings")]
    [SerializeField] private float throwForceMultiplier = 1f;

    [Header("Enemy Aim Settings")]
    [SerializeField] private float enemyTargetHeightOffset = 0.1f;

    public void OnThrow(InputValue value)
    {
        if (!value.isPressed) return;

        ThrowIngredient();
    }

    private void ThrowIngredient()
    {
        Debug.Log("Lanzando ingrediente");

        if (!CanThrow()) return;

        Vector3 throwDirection = GetCameraAimDirection();

        GameObject projectile = IngredientProjectilePool.Instance.GetProjectile(
            currentIngredient.projectilePrefab,
            throwPoint.position,
            Quaternion.LookRotation(throwDirection)
        );

        if (projectile == null) return;

        PrepareProjectile(projectile, throwDirection);
    }

    public void ThrowIngredientTowards(Vector3 targetPosition)
    {
        Debug.Log("Enemy throwing ingredient to player");

        if (!CanThrow()) return;

        // Apunta al cuerpo del jugador, no al piso ni hacia arriba exagerado
        Vector3 adjustedTarget = targetPosition + Vector3.up * enemyTargetHeightOffset;

        Vector3 direction = adjustedTarget - throwPoint.position;
        direction.Normalize();

        GameObject projectile = IngredientProjectilePool.Instance.GetProjectile(
            currentIngredient.projectilePrefab,
            throwPoint.position,
            Quaternion.LookRotation(direction)
        );

        if (projectile == null) return;

        PrepareProjectile(projectile, direction);
    }

    private void PrepareProjectile(GameObject projectile, Vector3 throwDirection)
    {
        AssignOwner(projectile);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            float finalThrowForce = currentIngredient.throwForce * throwForceMultiplier;

            rb.AddForce(throwDirection * finalThrowForce, ForceMode.Impulse);
        }

        PooledProjectile pooledProjectile = projectile.GetComponent<PooledProjectile>();

        if (pooledProjectile != null)
        {
            pooledProjectile.Activate(currentIngredient.lifeTime);
        }
    }

    private Vector3 GetCameraAimDirection()
    {
        if (cameraTransform == null)
        {
            return throwPoint.forward;
        }

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, aimDistance, aimLayerMask))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = cameraTransform.position + cameraTransform.forward * aimDistance;
        }

        Vector3 direction = targetPoint - throwPoint.position;
        direction.Normalize();

        return direction;
    }

    private bool CanThrow()
    {
        if (currentIngredient == null)
        {
            Debug.LogWarning("No hay ingrediente");
            return false;
        }

        if (currentIngredient.projectilePrefab == null)
        {
            Debug.LogWarning("El ingrediente no tiene prefab");
            return false;
        }

        if (throwPoint == null)
        {
            Debug.LogWarning("No hay ThrowPoint");
            return false;
        }

        if (IngredientProjectilePool.Instance == null)
        {
            Debug.LogWarning("No existe IngredientProjectilePool en la escena");
            return false;
        }

        return true;
    }

    private void AssignOwner(GameObject projectile)
    {
        IngredientDamage damageScript = projectile.GetComponent<IngredientDamage>();

        if (damageScript != null)
        {
            damageScript.SetOwner(gameObject);
        }
    }
}