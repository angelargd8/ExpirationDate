using UnityEngine;


[CreateAssetMenu(fileName = "New Ingredient", menuName = "Burguer Game/Ingredient")]
public class IngredientData: ScriptableObject
{
    [Header("Basic Info")]
    public string ingredientName;

    [Header("Projectile")]
    public GameObject projectilePrefab;
    public float throwForce = 15f;
    public float damage = 1f;
    public float lifeTime = 5f;


}
