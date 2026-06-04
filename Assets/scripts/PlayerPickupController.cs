using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickupController : MonoBehaviour
{
    [Header("Config de Pickups")]
    [SerializeField] private float pickupRadius = 1.5f;
    [SerializeField] private LayerMask pickupLayer;

    [Header("Referencias")]
    [SerializeField] private BurgerStats burgerStats;

    [Header("Audio PickUP")]
    [SerializeField] private AudioClip audioSFX;

    private PlayerInput playerInput;
    private InputAction interactAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        if (burgerStats == null)
        {
            burgerStats = GetComponentInChildren<BurgerStats>();
        }

        if (playerInput != null)
        {
            interactAction = playerInput.actions["Interact"];
            interactAction.Enable();
        }
        else
        {
            Debug.LogWarning("No se encontró PlayerInput en " + gameObject.name);
        }
    }

    private void Update()
    {
        if (interactAction == null) return;

        if (interactAction.WasPressedThisFrame())
        {
            Debug.Log("Se presiono F / Interact");
            TryPickup();
        }
    }

    private void TryPickup()
    {
        if (burgerStats == null)
        {
            Debug.LogWarning("No hay BurgerStats asignado en PlayerPickupController");
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            pickupRadius,
            pickupLayer
        );

        Debug.Log("Pickups detectados: " + colliders.Length);

        PickableIngredient closestIngredient = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            PickableIngredient ingredient = collider.GetComponentInParent<PickableIngredient>();

            if (ingredient == null)
            {
                Debug.Log("Collider sin PickableIngredient: " + collider.name);
                continue;
            }

            float distance = Vector3.Distance(transform.position, ingredient.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIngredient = ingredient;
            }
        }

        if (closestIngredient != null)
        {
            Debug.Log("Recogiendo ingrediente: " + closestIngredient.name);
            AudioManager.Instance.PlaySFX(audioSFX);
            closestIngredient.PickUp(burgerStats);
        }
        else
        {
            Debug.Log("No se encontro PickableIngredient cercano");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}