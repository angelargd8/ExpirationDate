using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelSceneReferences : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private BurgerStats playerStats;

    [Header("UI")]
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

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
            resultText,
            restartButton,
            quitButton
        );
    }
}