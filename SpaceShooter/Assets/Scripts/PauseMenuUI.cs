using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject root; // PauseOverlay panel root

    bool isOpen;

    void Awake()
    {
        if (!root) root = gameObject;
        root.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // If a "blocking" overlay is open, don't stack pause on top
            if (GameManagerUI.Instance != null && GameManagerUI.Instance.IsBlockingOverlayOpen)
                return;

            Toggle();
        }
    }

    public void Toggle()
    {
        if (isOpen) Close();
        else Open();
    }

    public void Open()
    {
        isOpen = true;
        root.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Close()
    {
        isOpen = false;
        root.SetActive(false);
        Time.timeScale = 1f;
    }

    // Hook to Resume button
    public void OnResumePressed() => Close();

    // Hook to Restart button
    public void OnRestartPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Optional quit button
    public void OnQuitPressed()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
