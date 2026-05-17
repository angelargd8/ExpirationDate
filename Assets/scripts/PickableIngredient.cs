using UnityEngine;

public class PickableIngredient : MonoBehaviour
{
    [Header("Datos del ingrediente")]
    [SerializeField] private IngredientDataPickUp ingredientData;

    [Header("Rotación visual")]
    [SerializeField] private bool rotate = true;
    [SerializeField] private float rotationSpeed = 90f;

    private bool wasPickedUp = false;

    private void Update()
    {
        if (!rotate) return;
        
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

    public void PickUp(BurgerStats burgerStats)
    {
        if (wasPickedUp) return;
        if (burgerStats == null) return;
        if (ingredientData == null) return;

        wasPickedUp = true;

        burgerStats.ApplyIngredient(ingredientData);

        if (ingredientData.destroyOnPickup)
        {
            Destroy(gameObject);
        }
    }
}