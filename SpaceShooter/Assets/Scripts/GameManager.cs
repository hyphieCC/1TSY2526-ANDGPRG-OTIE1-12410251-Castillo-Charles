using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [System.Serializable]
    public class LevelData
    {
        public int goalScore = 15; // points needed this level
        public float spawnInterval = 1.2f;
        public float enemySpeed = 6f;
        public float powerUpInterval = 8f;
    }

    [Header("Levels")]
    [SerializeField] private LevelData[] levels;
    int levelIndex = 0;

    [Header("References")]
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private PowerUpManager powerUpManager;

    bool levelRunning;
    int scoreAtLevelStart;
    int currentGoal; // how many points to gain this level

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        if (levels == null || levels.Length == 0)
        {
            Debug.LogWarning("GameManager: No levels configured.");
            return;
        }

        StartLevel(0);
    }

    void Update()
    {
        if (!levelRunning) return;

        var ui = GameManagerUI.Instance;
        if (ui == null) return;

        int gained = ui.Score - scoreAtLevelStart;
        GameManagerUI.Instance?.SetGoalProgress(gained, currentGoal);
        if (gained >= currentGoal)
            CompleteLevel();
    }

    public void StartLevel(int index)
    {
        GameManagerUI.Instance?.SetGoalProgress(0, currentGoal);

        levelIndex = Mathf.Clamp(index, 0, levels.Length - 1);
        var lvl = levels[levelIndex];

        // Apply difficulty
        if (enemyManager)
        {
            enemyManager.spawnInterval = lvl.spawnInterval;
            enemyManager.enemySpeedOverride = lvl.enemySpeed;
            enemyManager.enabled = true;
        }

        if (powerUpManager)
        {
            powerUpManager.spawnInterval = lvl.powerUpInterval;
            powerUpManager.enabled = true;
        }

        // Score tracking for THIS level only
        scoreAtLevelStart = GameManagerUI.Instance ? GameManagerUI.Instance.Score : 0;
        currentGoal = Mathf.Max(1, lvl.goalScore);

        levelRunning = true;

        GameManagerUI.Instance?.ShowLevel(levelIndex + 1);
        GameManagerUI.Instance?.SetGoalProgress(0, currentGoal);
        GameManagerUI.Instance?.HidePanels();
    }

    public void CompleteLevel()
    {
        if (!levelRunning) return;
        levelRunning = false;

        if (enemyManager) enemyManager.enabled = false;
        if (powerUpManager) powerUpManager.enabled = false;

        AudioManager.Instance?.PlaySfx(AudioManager.Instance.levelCompleteClip);

        if (levelIndex >= levels.Length - 1)
        {
            GameManagerUI.Instance?.ShowYouWin();
            return;
        }

        Invoke(nameof(ShowCompleteDelayed), 0.6f);
    }

    void ShowCompleteDelayed()
    {
        GameManagerUI.Instance?.ShowLevelComplete();
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        GameManagerUI.Instance?.HidePanels();

        // Clear leftovers so next level starts clean
        foreach (var e in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
            Destroy(e.gameObject);

        foreach (var p in FindObjectsByType<WeaponPowerUp>(FindObjectsSortMode.None))
            Destroy(p.gameObject);

        int next = levelIndex + 1;
        if (next >= levels.Length)
        {
            GameManagerUI.Instance?.ShowYouWin();
            return;
        }

        StartLevel(next);
    }

    public void GameOver()
    {
        levelRunning = false;
        if (enemyManager) enemyManager.enabled = false;
        if (powerUpManager) powerUpManager.enabled = false;

        GameManagerUI.Instance?.ShowGameOver();
    }

    public void OnScoreChanged(int newScore)
    {
        int gained = newScore - scoreAtLevelStart;

        GameManagerUI.Instance?.SetGoalProgress(gained, currentGoal);

        if (gained >= currentGoal)
            CompleteLevel();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
