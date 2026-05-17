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

        SetProjectileOwner(projectile);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(throwPoint.forward * currentIngredient.throwForce, ForceMode.Impulse);
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
            Debug.LogWarning("El ingrediente no tiene prefab");
            return false;
        }

        if (throwPoint == null)
        {
            Debug.LogWarning("No hay ThrowPoint");
            return false;
        }

        return true;
    }

    private void SetProjectileOwner(GameObject projectile)
    {
        EnemyIngredientDamage damageScript = projectile.GetComponent<EnemyIngredientDamage>();

        if (damageScript != null)
        {
            damageScript.SetOwner(gameObject);
        }
    }


    public void ThrowIngredientTowards(Vector3 targetPosition)
    {
        Debug.Log("Enemigo lanzando ingrediente hacia el jugador");

        if (currentIngredient == null)
        {
            Debug.LogWarning("No hay ingrediente ");
            return;
        }

        if (currentIngredient.projectilePrefab == null)
        {
            Debug.LogWarning("El ingrediente no tiene prefab ");
            return;
        }

        if (throwPoint == null)
        {
            Debug.LogWarning("No hay ThrowPoint ");
            return;
        }

        GameObject projectile = Instantiate(
            currentIngredient.projectilePrefab,
            throwPoint.position,
            Quaternion.identity
        );

        EnemyIngredientDamage damageScript = projectile.GetComponent<EnemyIngredientDamage>();

        if (damageScript != null)
        {
            damageScript.SetOwner(gameObject);
        }

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
}