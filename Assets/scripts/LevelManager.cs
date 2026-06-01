using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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

    [Header("Pause UI")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button pauseResumeButton;
    [SerializeField] private Button pauseRestartButton;
    [SerializeField] private Button pauseMenuButton;
    [SerializeField] private Button pauseQuitButton;

    [Header("Enemy Settings")]
    [SerializeField] private int totalEnemies = 20;
    [SerializeField] private int defeatedEnemies = 0;


    private int currentGameMinutes;
    private int endGameMinutes;
    private float timer;
    private bool gameEnded;
    private bool isPaused;

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
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }

        if (gameEnded) return;
        if (isPaused) return;
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
         TMP_Text newEnemiesText,
         GameObject newPausePanel,
         Button newPauseResumeButton,
         Button newPauseRestartButton,
         Button newPauseMenuButton,
         Button newPauseQuitButton
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

            pausePanel = newPausePanel;
            pauseResumeButton = newPauseResumeButton;
            pauseRestartButton = newPauseRestartButton;
            pauseMenuButton = newPauseMenuButton;
            pauseQuitButton = newPauseQuitButton;

            ConfigureButtons();
            ResetLevelState();
    }

    private void ConfigureButtons()
    {
        //panel de resultado
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

        // panel de pausa
        if (pauseResumeButton != null)
        {
            pauseResumeButton.onClick.RemoveAllListeners();
            pauseResumeButton.onClick.AddListener(ResumeGame);
        }

        if (pauseRestartButton != null)
        {
            pauseRestartButton.onClick.RemoveAllListeners();
            pauseRestartButton.onClick.AddListener(RestartLevel);
        }

        if (pauseMenuButton != null)
        {
            pauseMenuButton.onClick.RemoveAllListeners();
            pauseMenuButton.onClick.AddListener(Menu);
        }

        if (pauseQuitButton != null)
        {
            pauseQuitButton.onClick.RemoveAllListeners();
            pauseQuitButton.onClick.AddListener(QuitGame);
        }
    }

    private void ResetLevelState()
    {
        currentGameMinutes = startHour * 60;
        endGameMinutes = endHour * 60;

        timer = 0f;
        gameEnded = false;
        isPaused = false;
        defeatedEnemies = 0;

        Time.timeScale = 1f;

        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
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
        GameManager.instance.QuitGame();
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

    public void TogglePause()
    {
        if (gameEnded) return;

        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (gameEnded) return;

        isPaused = true;

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        if (gameEnded) return;

        isPaused = false;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}