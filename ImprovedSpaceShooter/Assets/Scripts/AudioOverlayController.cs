using UnityEngine;

//https://youtu.be/DU7cgVsU2rM?si=pMLtBDaR_dr-Nr9G

public class AudioOverlayController : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private bool pauseGameWhileOpen = false;

    bool isOpen;

    void Awake()
    {
        if (!panelRoot) 
            panelRoot = gameObject;

        panelRoot.SetActive(false);
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        if (!isOpen && GameManagerUI.Instance != null && GameManagerUI.Instance.IsBlockingOverlayOpen) return;

        Toggle();
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
        panelRoot.SetActive(true);

        var settings = panelRoot.GetComponentInChildren<AudioSettingsUI>(true);
        if (settings) 
            settings.SyncFromPrefsAndApplyPublic();

        if (pauseGameWhileOpen) 
            Time.timeScale = 0f;
    }

    public void Close()
    {
        isOpen = false;
        panelRoot.SetActive(false);

        if (pauseGameWhileOpen) 
            Time.timeScale = 1f;
    }
}
