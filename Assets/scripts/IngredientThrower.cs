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

        if (currentIngredient == null)
        {
            Debug.LogWarning("No hay ingrediente asignado.");
            return;
        }

        if (currentIngredient.projectilePrefab == null)
        {
            Debug.LogWarning("El ingrediente no tiene prefab asignado.");
            return;
        }

        if (throwPoint == null)
        {
            Debug.LogWarning("No hay ThrowPoint asignado.");
            return;
        }

        GameObject projectile = Instantiate(
            currentIngredient.projectilePrefab,
            throwPoint.position,
            throwPoint.rotation
        );

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(throwPoint.forward * currentIngredient.throwForce, ForceMode.Impulse);
        }

        Destroy(projectile, currentIngredient.lifeTime);
    }
}