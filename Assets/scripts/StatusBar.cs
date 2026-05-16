using UnityEngine;
using UnityEngine.UI;

public class StatusBars : MonoBehaviour
{
    [Header("Barras")]
    [SerializeField] private Image lifeBarFill;
    [SerializeField] private Image freshnessBarFill;

    [Header("Objetivo")]
    [SerializeField] private BurgerStats targetStats;

    [Header("Camara")]
    [SerializeField] private Transform cameraTransform;

    private void Start()
    {
        if (targetStats == null)
        {
            targetStats = GetComponentInParent<BurgerStats>();
        }

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        UpdateBars();
    }

    private void LateUpdate()
    {
        UpdateBars();
        FaceCamera();
    }

    private void UpdateBars()
    {
        if (targetStats == null)
        {
            return;
        }

        if (lifeBarFill != null)
        {
            lifeBarFill.fillAmount = targetStats.GetLifePercentage();
        }

        if (freshnessBarFill != null)
        {
            freshnessBarFill.fillAmount = targetStats.GetFreshnessPercentage();
        }
    }

    private void FaceCamera()
    {
        if (cameraTransform == null) return;

        transform.LookAt(transform.position + cameraTransform.forward);
    }
}