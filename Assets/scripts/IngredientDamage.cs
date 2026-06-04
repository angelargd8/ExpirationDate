using System.Collections.Generic;
using UnityEngine;

public class IngredientDamage : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int damage = 10;

    [Header("SFX")]
    [SerializeField] private AudioClip audioSFX;

    private GameObject owner;
    private bool hasHit = false;

    private Collider[] projectileColliders;
    private readonly List<Collider> ignoredOwnerColliders = new List<Collider>();

    private void Awake()
    {
        projectileColliders = GetComponentsInChildren<Collider>();
    }

    public void SetOwner(GameObject newOwner)
    {
        ClearIgnoredOwnerColliders();

        owner = newOwner;
        hasHit = false;

        IgnoreOwnerColliders();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;

        // Ignora al dueño del proyectil
        if (owner != null && collision.transform.root == owner.transform.root)
        {
            return;
        }

        BurgerStats stats = collision.gameObject.GetComponentInParent<BurgerStats>();

        // Si no golpeó jugador/enemigo, NO desaparece
        if (stats == null)
        {
            Debug.Log(gameObject.name + " chocó con " + collision.gameObject.name + ", pero no hizo daño.");
            return;
        }

        hasHit = true;

        stats.TakeDamage(damage);

        if (AudioManager.Instance != null && audioSFX != null)
        {
            AudioManager.Instance.PlaySFX(audioSFX);
        }

        Debug.Log(gameObject.name + " hizo " + damage + " de daño a " + stats.gameObject.name);

        ReturnToPool();
    }

    private void IgnoreOwnerColliders()
    {
        if (owner == null) return;

        Collider[] ownerColliders = owner.GetComponentsInChildren<Collider>();

        foreach (Collider projectileCollider in projectileColliders)
        {
            foreach (Collider ownerCollider in ownerColliders)
            {
                if (projectileCollider == null || ownerCollider == null) continue;

                Physics.IgnoreCollision(projectileCollider, ownerCollider, true);
                ignoredOwnerColliders.Add(ownerCollider);
            }
        }
    }

    private void ClearIgnoredOwnerColliders()
    {
        if (projectileColliders == null) return;

        foreach (Collider projectileCollider in projectileColliders)
        {
            foreach (Collider ownerCollider in ignoredOwnerColliders)
            {
                if (projectileCollider == null || ownerCollider == null) continue;

                Physics.IgnoreCollision(projectileCollider, ownerCollider, false);
            }
        }

        ignoredOwnerColliders.Clear();
    }

    private void ReturnToPool()
    {
        PooledProjectile pooledProjectile = GetComponent<PooledProjectile>();

        if (pooledProjectile != null)
        {
            pooledProjectile.ReturnToPool();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        ClearIgnoredOwnerColliders();
    }
}