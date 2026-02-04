using TMPro;
using UnityEngine;

public class GameManagerUI : MonoBehaviour
{
    public static GameManagerUI Instance;

    [Header("HUD")]
    [SerializeField] private TMP_Text modeText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text goalText;
    [SerializeField] TMP_Text hpText;

    [Header("Panels (optional)")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private GameObject youWinPanel;

    public bool IsBlockingOverlayOpen =>
    (gameOverPanel && gameOverPanel.activeSelf) ||
    (levelCompletePanel && levelCompletePanel.activeSelf) ||
    (youWinPanel && youWinPanel.activeSelf);

    int score = 0;
    public int Score => score;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        HidePanels();
        SetMode("Single (1)");
    }

    public void AddScore(int amount)
    {
        score += amount;

        GameManager.Instance?.OnScoreChanged(score);
    }

    public void SetGoalProgress(int gained, int goal)
    {
        if (goalText)
            goalText.text = "Goal: " + gained + " / " + goal;
    }

    public void SetMode(string modeLabel)
    {
        if (modeText)
            modeText.text = "Mode: " + modeLabel;
    }

    public void ShowLevel(int levelNumber)
    {
        if (levelText)
            levelText.text = "Level: " + levelNumber;
    }

    public void HidePanels()
    {
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (levelCompletePanel) levelCompletePanel.SetActive(false);
        if (youWinPanel) youWinPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        if (gameOverPanel && !gameOverPanel.activeSelf)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ShowLevelComplete()
    {
        if (levelCompletePanel) levelCompletePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowYouWin()
    {
        HidePanels();
        if (youWinPanel) youWinPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void SetHP(int hp)
    {
        if (hpText)
            hpText.text = "HP: " + hp;
    }
}
