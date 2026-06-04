using System.Collections.Generic;
using UnityEngine;

public class IngredientProjectilePool : MonoBehaviour
{
    public static IngredientProjectilePool Instance { get; private set; }

    [Header("Pool Settings")]
    [SerializeField] private int defaultPoolSize = 30;

    private Dictionary<GameObject, Queue<GameObject>> pools = new Dictionary<GameObject, Queue<GameObject>>();
    private Dictionary<GameObject, GameObject> prefabLookup = new Dictionary<GameObject, GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject GetProjectile(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null)
        {
            Debug.LogWarning("Prefab de proyectil es null");
            return null;
        }

        if (!pools.ContainsKey(prefab))
        {
            CreatePool(prefab, defaultPoolSize);
        }

        GameObject projectile;

        if (pools[prefab].Count > 0)
        {
            projectile = pools[prefab].Dequeue();
        }
        else
        {
            projectile = CreateProjectile(prefab);
        }

        projectile.transform.SetPositionAndRotation(position, rotation);
        projectile.SetActive(true);

        return projectile;
    }

    public void ReturnProjectile(GameObject projectile)
    {
        if (projectile == null) return;

        projectile.SetActive(false);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (!prefabLookup.ContainsKey(projectile))
        {
            Debug.LogWarning("El proyectil no pertenece a ningún pool.");
            Destroy(projectile);
            return;
        }

        GameObject prefab = prefabLookup[projectile];

        pools[prefab].Enqueue(projectile);
    }

    private void CreatePool(GameObject prefab, int amount)
    {
        pools[prefab] = new Queue<GameObject>();

        for (int i = 0; i < amount; i++)
        {
            GameObject projectile = CreateProjectile(prefab);
            projectile.SetActive(false);
            pools[prefab].Enqueue(projectile);
        }
    }

    private GameObject CreateProjectile(GameObject prefab)
    {
        GameObject projectile = Instantiate(prefab, transform);
        prefabLookup[projectile] = prefab;

        PooledProjectile pooledProjectile = projectile.GetComponent<PooledProjectile>();

        if (pooledProjectile == null)
        {
            pooledProjectile = projectile.AddComponent<PooledProjectile>();
        }

        return projectile;
    }
}