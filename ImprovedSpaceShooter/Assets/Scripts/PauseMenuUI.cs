using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject root;

    bool isOpen;

    void Awake()
    {
        if (!root) 
            root = gameObject;

        root.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManagerUI.Instance != null && GameManagerUI.Instance.IsBlockingOverlayOpen)
                return;

            Toggle();
        }
    }

    public void Toggle()
    {
        if (isOpen) 
            Close();
        else 
            Open();
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

    public void OnResumePressed() => Close();

    public void OnRestartPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnQuitPressed()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
