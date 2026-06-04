using System.Collections;
using UnityEngine;

public class PooledProjectile : MonoBehaviour
{
    private Coroutine returnCoroutine;

    public void Activate(float lifeTime)
    {
        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
        }

        returnCoroutine = StartCoroutine(ReturnAfterTime(lifeTime));
    }

    public void ReturnToPool()
    {
        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
            returnCoroutine = null;
        }

        if (IngredientProjectilePool.Instance != null)
        {
            IngredientProjectilePool.Instance.ReturnProjectile(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator ReturnAfterTime(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);

        ReturnToPool();
    }
}