using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelSceneReferences : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private BurgerStats playerStats;

    [Header("Result UI")]
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private TMP_Text enemiesText;

    [Header("Pause UI")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button pauseResumeButton;
    [SerializeField] private Button pauseRestartButton;
    [SerializeField] private Button pauseMenuButton;
    [SerializeField] private Button pauseQuitButton;

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
            quitButton,
            menuButton,
            enemiesText,
            pausePanel,
            pauseResumeButton,
            pauseRestartButton,
            pauseMenuButton,
            pauseQuitButton
        );
    }
}