using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Player")]
    [SerializeField] private BurgerStats playerStats;

    [Header("Time Settings")]
    [SerializeField] private int startHour = 19;
    [SerializeField] private int endHour = 24;
    [SerializeField] private float secondsPerGameMinute = 1f;

    [Header("Win / Lose Settings")]
    [SerializeField] private float requiredFreshnessPercentage = 0.6f;

    [Header("UI")]
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private TMP_Text enemiesText;

    [Header("Enemy Settings")]
    [SerializeField] private int totalEnemies = 20;
    [SerializeField] private int defeatedEnemies = 0;


    private int currentGameMinutes;
    private int endGameMinutes;
    private float timer;
    private bool gameEnded;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {
        if (gameEnded) return;
        if (timeText == null) return;

        timer += Time.deltaTime;

        if (timer >= secondsPerGameMinute)
        {
            timer = 0f;
            AdvanceOneGameMinute();
        }
    }

    public void RegisterLevelReferences(
        BurgerStats newPlayerStats,
        TMP_Text newTimeText,
        GameObject newResultPanel,
        TMP_Text newResultText,
        Button newRestartButton,
        Button newQuitButton,
        Button newMenuButton,
        TMP_Text newEnemiesText
    )
    {
        playerStats = newPlayerStats;
        timeText = newTimeText;
        resultPanel = newResultPanel;
        resultText = newResultText;
        restartButton = newRestartButton;
        quitButton = newQuitButton;
        menuButton = newMenuButton;
        enemiesText = newEnemiesText;

        ConfigureButtons();
        ResetLevelState();
    }

    private void ConfigureButtons()
    {
        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(RestartLevel);
        }

        if (menuButton != null)
        {
            menuButton.onClick.RemoveAllListeners();
            menuButton.onClick.AddListener(Menu);
        }

        if (quitButton != null)
        {
            quitButton.onClick.RemoveAllListeners();
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    private void ResetLevelState()
    {
        currentGameMinutes = startHour * 60;
        endGameMinutes = endHour * 60;

        timer = 0f;
        gameEnded = false;
        defeatedEnemies = 0;

        Time.timeScale = 1f;

        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        UpdateTimeUI();
        UpdateEnemiesUI();
    }

    private void AdvanceOneGameMinute()
    {
        currentGameMinutes++;

        UpdateTimeUI();

        if (currentGameMinutes >= endGameMinutes)
        {
            CheckFinalResult();
        }
    }

    private void CheckFinalResult()
    {
        if (playerStats == null)
        {
            Debug.LogWarning("No hay PlayerStats asignado en LevelManager.");
            ShowResult(false);
            return;
        }

        float freshnessPercentage = playerStats.GetFreshnessPercentage();

        bool hasEnoughFreshness = freshnessPercentage >= requiredFreshnessPercentage;
        bool allEnemiesDefeated = defeatedEnemies >= totalEnemies;

        Debug.Log("Frescura suficiente: " + hasEnoughFreshness);
        Debug.Log("Todos los enemigos derrotados: " + allEnemiesDefeated);

        if (hasEnoughFreshness && allEnemiesDefeated)
        {
            ShowResult(true);
        }
        else
        {
            ShowResult(false);
        }
    }

    private void ShowResult(bool playerWon)
    {
        gameEnded = true;

        if (resultPanel != null)
        {
            resultPanel.SetActive(true);
        }

        if (resultText != null)
        {
            resultText.text = playerWon ? "YOU WIN" : "YOU LOSE";
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    private void UpdateTimeUI()
    {
        if (timeText == null) return;

        timeText.text = "TIME: " + FormatGameTime(currentGameMinutes);
    }

    private string FormatGameTime(int totalMinutes)
    {
        int hour24 = totalMinutes / 60;
        int minutes = totalMinutes % 60;

        string period = hour24 >= 12 && hour24 < 24 ? "PM" : "AM";

        int hour12 = hour24 % 12;

        if (hour12 == 0)
        {
            hour12 = 12;
        }

        return hour12.ToString("00") + ":" + minutes.ToString("00") + " " + period;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        defeatedEnemies = 0;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        defeatedEnemies = 0;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;

        Debug.Log("Quit Game");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void LoseGame()
    {
        ShowResult(false);
    }

    public void WinGame()
    {
        ShowResult(true);
    }


    public void RegisterEnemyDefeated()
    {
        if (gameEnded) return;

        defeatedEnemies++;
        defeatedEnemies = Mathf.Clamp(defeatedEnemies, 0, totalEnemies);

        UpdateEnemiesUI();

        Debug.Log("Enemigos derrotados: " + defeatedEnemies + " / " + totalEnemies);
    }

    private void UpdateEnemiesUI()
    {
        if (enemiesText == null) return;

        enemiesText.text = "ENEMIES: " + defeatedEnemies + " / " + totalEnemies;
    }

}