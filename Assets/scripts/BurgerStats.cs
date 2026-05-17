using UnityEngine;

public class BurgerStats : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private int maxLife = 100;
    [SerializeField] private int currentLife = 100;

    [Header("Frescura")]
    [SerializeField] private int maxFreshness = 100;
    [SerializeField] private int currentFreshness = 100;

    [Header("Frescura automatica")]
    [SerializeField] private bool reduceFreshnessOverTime = false;
    [SerializeField] private float freshnessLossInterval = 1f;
    [SerializeField] private int freshnessLossAmount = 1;

    private float freshnessTimer;

    private void Awake()
    {
        currentLife = Mathf.Clamp(currentLife, 0, maxLife);
        currentFreshness = Mathf.Clamp(currentFreshness, 0, maxFreshness);
    }

    private void Update()
    {
        if (!reduceFreshnessOverTime) return;

        freshnessTimer += Time.deltaTime;

        if (freshnessTimer >= freshnessLossInterval)
        {
            ReduceFreshness(freshnessLossAmount);
            freshnessTimer = 0f;
        }
    }

    public float GetLifePercentage()
    {
        if (maxLife <= 0) return 0f;
        return (float)currentLife / maxLife;
    }

    public float GetFreshnessPercentage()
    {
        if (maxFreshness <= 0) return 0f;
        return (float)currentFreshness / maxFreshness;
    }



    public void Heal(int amount)
    {
        currentLife += amount;
        currentLife = Mathf.Clamp(currentLife, 0, maxLife);
    }

    public void ReduceFreshness(int amount)
    {
        currentFreshness -= amount;
        currentFreshness = Mathf.Clamp(currentFreshness, 0, maxFreshness);
    }

    public void AddFreshness(int amount)
    {
        currentFreshness += amount;
        currentFreshness = Mathf.Clamp(currentFreshness, 0, maxFreshness);
    }

    public void TakeDamage(int amount)
    {
        currentLife -= amount;
        currentLife = Mathf.Clamp(currentLife, 0, maxLife);

        Debug.Log(gameObject.name + " recibio daño, vida actual: " + currentLife);

        if (currentLife <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " fue derrotado");

      
        gameObject.SetActive(false);
    }


    public void ApplyIngredient(IngredientDataPickUp ingredientData)
    {
        if (ingredientData == null) return;

        if (ingredientData.lifeAmount > 0)
        {
            Heal(ingredientData.lifeAmount);
        }
        else if (ingredientData.lifeAmount < 0)
        {
            TakeDamage(Mathf.Abs(ingredientData.lifeAmount));
        }

        if (ingredientData.freshnessAmount > 0)
        {
            AddFreshness(ingredientData.freshnessAmount);
        }
        else if (ingredientData.freshnessAmount < 0)
        {
            ReduceFreshness(Mathf.Abs(ingredientData.freshnessAmount));
        }

        Debug.Log(gameObject.name + " recogio " + ingredientData.ingredientName);
    }

    private void OnValidate()
    {
        currentLife = Mathf.Clamp(currentLife, 0, maxLife);
        currentFreshness = Mathf.Clamp(currentFreshness, 0, maxFreshness);
    }
}