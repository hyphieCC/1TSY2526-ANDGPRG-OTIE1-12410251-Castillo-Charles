using UnityEngine;
using TMPro;

public class StealPromptUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private void Awake()
    {
        if (text == null) text = GetComponentInChildren<TMP_Text>();
        Clear();
    }

    public void Show(string msg)
    {
        if (text == null) return;
        text.text = msg;
    }

    public void Clear()
    {
        if (text == null) return;
        text.text = "";
    }
}
