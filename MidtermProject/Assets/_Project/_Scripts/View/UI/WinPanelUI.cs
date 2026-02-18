using UnityEngine;
using UnityEngine.UI;

public class WinPanelUI : MonoBehaviour
{
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button restartButton;

    public void ShowNext()
    {
        if (nextLevelButton != null) nextLevelButton.gameObject.SetActive(true);
        if (restartButton != null) restartButton.gameObject.SetActive(false);
    }

    public void ShowRestart()
    {
        if (nextLevelButton != null) nextLevelButton.gameObject.SetActive(false);
        if (restartButton != null) restartButton.gameObject.SetActive(true);
    }
}
