using TMPro;
using UnityEngine;

public class GameManagerUI : MonoBehaviour
{
    public static GameManagerUI Instance;

    public TMP_Text scoreText;
    public TMP_Text modeText;

    int score = 0;

    void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
            return; 
        }

        Instance = this;
    }

    void Start()
    {
        UpdateScoreUI();
        SetMode("Single (1)");
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText) 
            scoreText.text = "Score: " + score;
    }

    public void SetMode(string modeLabel)
    {
        if (modeText)
            modeText.text = "Mode: " + modeLabel;
    }
}