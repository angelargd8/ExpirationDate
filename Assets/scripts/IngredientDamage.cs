using UnityEngine;

public class IngredientDamage : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int damage = 10;

    [Header("SFX")]
    [SerializeField] private AudioClip audioSFX;

    private GameObject owner;

    public void SetOwner(GameObject newOwner)
    {
        owner = newOwner;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // no evitar a quien lo produjo
        if (owner != null && collision.transform.root == owner.transform.root)
        {
            return;
        }

        BurgerStats stats = collision.gameObject.GetComponentInParent<BurgerStats>();
        

        if (stats != null)
        {
            
            stats.TakeDamage(damage);
            AudioManager.Instance.PlaySFX(audioSFX);

            Debug.Log(gameObject.name + " hizo " + damage + " de daño a " + stats.gameObject.name);

            return;
        }


    }
}