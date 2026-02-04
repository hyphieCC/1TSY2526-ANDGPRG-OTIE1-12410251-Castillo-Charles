using UnityEngine;

public class AudioOverlayController : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private bool pauseGameWhileOpen = false; // toggle this as you like

    bool isOpen;

    void Awake()
    {
        if (!panelRoot) panelRoot = gameObject;
        panelRoot.SetActive(false);
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        // Don't open on top of GameOver/LevelComplete/YouWin
        if (!isOpen && GameManagerUI.Instance != null && GameManagerUI.Instance.IsBlockingOverlayOpen)
            return;

        Toggle();
    }

    public void Toggle()
    {
        if (isOpen) Close();
        else Open();
    }

    public void Open()
    {
        isOpen = true;
        panelRoot.SetActive(true);

        // Sync sliders/toggle from saved prefs and apply audio
        var settings = panelRoot.GetComponentInChildren<AudioSettingsUI>(true);
        if (settings) settings.SyncFromPrefsAndApplyPublic();

        if (pauseGameWhileOpen) Time.timeScale = 0f;
    }

    public void Close()
    {
        isOpen = false;
        panelRoot.SetActive(false);

        if (pauseGameWhileOpen) Time.timeScale = 1f;
    }
}
