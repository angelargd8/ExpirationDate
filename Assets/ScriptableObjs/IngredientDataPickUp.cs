using UnityEngine;

public enum IngredientType
{
    Cheese,
    Lettuce,
    Tomato,
    Patty
}

[CreateAssetMenu(fileName = "NewIngredientData", menuName = "Burger Game/Ingredient Data PickUP")]

public class IngredientDataPickUp : ScriptableObject
{
    [Header("Información")]
    public string ingredientName;
    public IngredientType ingredientType;

    [Header("Efectos")]
    public int lifeAmount;
    public int freshnessAmount;

    [Header("Configuración")]
    public bool destroyOnPickup = true;
}