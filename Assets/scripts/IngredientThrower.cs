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

        GameObject projectile = Instantiate(
            currentIngredient.projectilePrefab,
            throwPoint.position,
            Quaternion.LookRotation(throwDirection)
        );

        AssignOwner(projectile);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            float finalThrowForce = currentIngredient.throwForce * throwForceMultiplier;

            rb.AddForce(throwDirection * finalThrowForce, ForceMode.Impulse);
        }

        Destroy(projectile, currentIngredient.lifeTime);
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

    public void ThrowIngredientTowards(Vector3 targetPosition)
    {
        Debug.Log("Enemy throwing ingredient to player");

        if (!CanThrow()) return;

        GameObject projectile = Instantiate(
            currentIngredient.projectilePrefab,
            throwPoint.position,
            Quaternion.identity
        );

        AssignOwner(projectile);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            Vector3 direction = targetPosition - throwPoint.position;

            // Altura del arco del lanzamiento 
            direction.y += 1.2f;

            direction.Normalize();

            float finalThrowForce = currentIngredient.throwForce * throwForceMultiplier;

            rb.AddForce(direction * finalThrowForce, ForceMode.Impulse);
        }

        Destroy(projectile, currentIngredient.lifeTime);
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
            Debug.LogWarning("El ingrediente no tiene prefab ");
            return false;
        }

        if (throwPoint == null)
        {
            Debug.LogWarning("No hay ThrowPoint");
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