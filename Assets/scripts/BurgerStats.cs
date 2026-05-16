using UnityEngine;

public class BurgerStats : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private int maxLife = 100;
    [SerializeField] private int currentLife = 100;

    [Header("Frescura")]
    [SerializeField] private int maxFreshness = 100;
    [SerializeField] private float currentFreshness = 100f;

    [Header("Frescura automatica")]
    [SerializeField] private bool reduceFreshnessOverTime = false;
    [SerializeField] private float freshnessLossInterval = 1f;
    [SerializeField] private float freshnessLossAmount = 0.1f;

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

    public void TakeDamage(int amount)
    {
        currentLife -= amount;
        currentLife = Mathf.Clamp(currentLife, 0, maxLife);
    }

    public void Heal(int amount)
    {
        currentLife += amount;
        currentLife = Mathf.Clamp(currentLife, 0, maxLife);
    }

    public void ReduceFreshness(float amount)
    {
        currentFreshness -= amount;
        currentFreshness = Mathf.Clamp(currentFreshness, 0, maxFreshness);
    }

    public void AddFreshness(int amount)
    {
        currentFreshness += amount;
        currentFreshness = Mathf.Clamp(currentFreshness, 0, maxFreshness);
    }

    private void OnValidate()
    {
        currentLife = Mathf.Clamp(currentLife, 0, maxLife);
        currentFreshness = Mathf.Clamp(currentFreshness, 0, maxFreshness);
    }
}