using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseFlow : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private AlertSystem alertSystem;

    [Header("UI Panels")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private WinPanelUI winPanelUI;
    [SerializeField] private GameObject losePanel;

    [Header("Freeze During End Screen")]
    [SerializeField] private bool pauseTime = true;

    private bool ended;

    private void Awake()
    {
        if (alertSystem == null) alertSystem = FindFirstObjectByType<AlertSystem>();

        if (winPanel != null) winPanel.SetActive(false);
        if (winPanelUI == null && winPanel != null) winPanelUI = winPanel.GetComponent<WinPanelUI>();
        if (losePanel != null) losePanel.SetActive(false);

        ended = false;
    }

    private void Update()
    {
        if (ended) return;

        if (alertSystem != null && alertSystem.IsLost)
        {
            Lose();
        }
    }

    public void Win()
    {
        if (ended) return;
        ended = true;

        AudioManager.Instance?.Play2D(SoundType.ExtractionSuccess, 1f);

        if (winPanel != null) winPanel.SetActive(true);

        int current = SceneManager.GetActiveScene().buildIndex;
        int last = SceneManager.sceneCountInBuildSettings - 1;
        bool hasNext = current < last;

        if (winPanelUI != null)
        {
            if (hasNext) winPanelUI.ShowNext();
            else winPanelUI.ShowRestart();
        }

        EndFreeze();
    }

    public void Lose()
    {
        if (ended) return;
        ended = true;

        if (losePanel != null) losePanel.SetActive(true);
        EndFreeze();
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;

        int current = SceneManager.GetActiveScene().buildIndex;
        int last = SceneManager.sceneCountInBuildSettings - 1;

        if (current < last)
            SceneManager.LoadScene(current + 1);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void EndFreeze()
    {
        if (pauseTime) Time.timeScale = 0f;
    }
}
