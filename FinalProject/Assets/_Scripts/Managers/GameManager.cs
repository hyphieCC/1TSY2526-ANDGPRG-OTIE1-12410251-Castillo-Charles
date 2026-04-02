using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] TMP_Text currentWave;

    [Header("Gold")]
    [SerializeField] int startingGold = 300;
    [SerializeField] TMP_Text goldText;
    int currentGold;

    public Transform GetCoreTransform() { return coreData.coreTransform; }

    private void Awake()
    {
        Instance = this;
        coreData.curlife = coreData.maxLife;
        currentGold = startingGold;

        UpdateLifeBar();
        UpdateGoldUI();
    }

    public void CoreTakeDamage(int damage)
    {
        coreData.curlife -= damage;
        coreData.curlife = Mathf.Max(coreData.curlife, 0);
        UpdateLifeBar();

        if (coreData.curlife <= 0)
        {
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
        Debug.Log("GameOver");
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

    public void UpdateWaveUI(int wave)
    {
        currentWave.text = "Wave: " + wave;
    }
}