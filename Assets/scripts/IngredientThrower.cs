using UnityEngine;
using UnityEngine.InputSystem;

public class IngredientThrower : MonoBehaviour
{
    [Header("Ingredient")]
    [SerializeField] private IngredientData currentIngredient;

    [Header("Throw Settings")]
    [SerializeField] private Transform throwPoint;

    public void OnThrow(InputValue value)
    {
        if (!value.isPressed) return;

        ThrowIngredient();
    }

    private void ThrowIngredient()
    {
        Debug.Log("Lanzando ingrediente");

        if (!CanThrow()) return;

        GameObject projectile = Instantiate(
            currentIngredient.projectilePrefab,
            throwPoint.position,
            throwPoint.rotation
        );

        AssignOwner(projectile);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(throwPoint.forward * currentIngredient.throwForce, ForceMode.Impulse);
        }

        Destroy(projectile, currentIngredient.lifeTime);
    }

    public void ThrowIngredientTowards(Vector3 targetPosition)
    {
        Debug.Log("Enemigo lanzando ingrediente hacia el jugador");

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

            Vector3 direction = targetPosition - throwPoint.position;
            direction.y += 1.2f;
            direction.Normalize();

            rb.AddForce(direction * currentIngredient.throwForce, ForceMode.Impulse);
        }

        Destroy(projectile, currentIngredient.lifeTime);
    }

    private bool CanThrow()
    {

        if (currentIngredient == null)
        {
            Debug.LogWarning("No hay ingrediente asignado.");
            return false;
        }

        if (currentIngredient.projectilePrefab == null)
        {
            Debug.LogWarning("El ingrediente no tiene prefab asignado.");
            return false;
        }

        if (throwPoint == null)
        {
            Debug.LogWarning("No hay ThrowPoint asignado.");
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