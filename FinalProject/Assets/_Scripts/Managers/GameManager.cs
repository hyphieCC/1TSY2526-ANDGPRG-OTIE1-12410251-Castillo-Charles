using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class CoreData
{
    public Transform coreTransform;
    public float curlife;
    public float maxLife;
    public Image lifeBar;
}

public class GameManager : MonoBehaviour
{
    // Making an Instance of an object as a Static Instance/ Single instancing
    // that must be one of the following
    // Manager, Handler, Controller
    // != Cannot use it to Player, Enemies or any object that has a possibility will have a multiple instance
    public static GameManager Instance;

    [SerializeField] CoreData coreData;

    [Header("Gold")]
    [SerializeField] int startingGold = 300;
    [SerializeField] TMP_Text goldText;
    int currentGold;

    [Header("BGM")]
    [SerializeField] AudioClip levelMusic;

    [Header("End Game UI")]
    [SerializeField] GameObject losePanel;
    [SerializeField] GameObject winPanel;

    public Transform GetCoreTransform() { return coreData.coreTransform; }

    private void Awake()
    {
        Instance = this;
        coreData.curlife = coreData.maxLife;
        currentGold = startingGold;

        losePanel.SetActive(false);
        winPanel.SetActive(false);

        UpdateLifeBar();
        UpdateGoldUI();
    }

    private void Start()
    {
        SoundManager.Instance.PlayMusic(levelMusic);
    }

    public void CoreTakeDamage(int damage)
    {
        SoundManager.Instance.PlaySFX3D(SoundManager.SFXType.CoreHit, transform.position);
        coreData.curlife -= damage;
        coreData.curlife = Mathf.Max(coreData.curlife, 0);
        UpdateLifeBar();

        if (coreData.curlife <= 0)
        {
            SoundManager.Instance.PlaySFX3D(SoundManager.SFXType.CoreDeath, transform.position);
            GameOver();
        }
    }

    public int GetGold()
    {
        return currentGold;
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        UpdateGoldUI();
        //Debug.Log("Gold Added: " + amount + " | Current Gold: " + currentGold);
    }

    public bool SpendGold(int amount)
    {
        if (currentGold < amount)
        {
            return false;
        }

        currentGold -= amount;
        UpdateGoldUI();
        //Debug.Log("Gold Spent: " + amount + " | Current Gold: " + currentGold);
        return true;
    }

    void GameOver()
    {
        losePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void UpdateLifeBar()
    {
        coreData.lifeBar.fillAmount = coreData.curlife / coreData.maxLife;
    }

    void UpdateGoldUI()
    {
        if (goldText != null)
        {
            goldText.text = "Gold: " + currentGold;
        }
    }

    public void WinGame()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}