using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
        TMP_Text newResultText
    )
    {
        playerStats = newPlayerStats;
        timeText = newTimeText;
        resultPanel = newResultPanel;
        resultText = newResultText;

        ResetLevelState();
    }

    private void ResetLevelState()
    {
        currentGameMinutes = startHour * 60;
        endGameMinutes = endHour * 60;

        timer = 0f;
        gameEnded = false;

        Time.timeScale = 1f;

        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        UpdateTimeUI();
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

        if (freshnessPercentage >= requiredFreshnessPercentage)
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

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;

        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void LoseGame()
    {
        ShowResult(false);
    }

    public void WinGame()
    {
        ShowResult(true);
    }

}   
