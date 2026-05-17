using UnityEngine;

public class EnemyIngredientDamage : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int damage = 10;

    private GameObject owner;

    public void SetOwner(GameObject newOwner)
    {
        owner = newOwner;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == owner)
        {
            return;
        }

        BurgerStats stats = collision.gameObject.GetComponent<BurgerStats>();

        if (stats != null)
        {
            stats.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }
    }
}