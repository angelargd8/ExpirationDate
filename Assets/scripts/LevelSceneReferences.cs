using UnityEngine;
using TMPro;

public class LevelSceneReferences : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private BurgerStats playerStats;

    [Header("UI")]
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text resultText;

    private void Start()
    {
        if (LevelManager.Instance == null)
        {
            Debug.LogWarning("No existe LevelManager.Instance.");
            return;
        }

        LevelManager.Instance.RegisterLevelReferences(
            playerStats,
            timeText,
            resultPanel,
            resultText
        );
    }
}