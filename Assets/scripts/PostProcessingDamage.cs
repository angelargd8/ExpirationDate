using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingDamage : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Volume globalVolume;
    [SerializeField] private BurgerStats playerStats;

    [Header("Configuracion de Vignette")]
    [SerializeField] private float lifePercentageToStart = 0.6f;
    [SerializeField] private float maxVignetteIntensity = 0.8f;
    [SerializeField] private float transitionSpeed = 0.5f;

    private Vignette vignette;
    private float targetIntensity;

    private void Awake()
    {
        if (globalVolume == null)
        {
            globalVolume = GetComponent<Volume>();
        }

        if (globalVolume == null)
        {
            Debug.LogWarning("No se encontro el componente Volume en " + gameObject.name);
            return;
        }

        if (globalVolume.profile == null)
        {
            Debug.LogWarning("El Global Volume no tiene un Profile");
            return;
        }

        if (!globalVolume.profile.TryGet(out vignette))
        {
            Debug.LogWarning("no hay Vignette");
            return;
        }

        vignette.intensity.value = 0f;
    }

    private void Update()
    {
        if (playerStats == null) return;
        if (vignette == null) return;

        UpdateVignetteByLife();
    }

    private void UpdateVignetteByLife()
    {
        float lifePercentage = playerStats.GetLifePercentage();

        if (lifePercentage > lifePercentageToStart)
        {
            targetIntensity = 0f;
        }
        else
        {
            float damageProgress = Mathf.InverseLerp(lifePercentageToStart, 0f, lifePercentage);
            targetIntensity = Mathf.Lerp(0f, maxVignetteIntensity, damageProgress);
        }

        vignette.intensity.value = Mathf.MoveTowards(
            vignette.intensity.value,
            targetIntensity,
            transitionSpeed * Time.deltaTime
        );
    }
}