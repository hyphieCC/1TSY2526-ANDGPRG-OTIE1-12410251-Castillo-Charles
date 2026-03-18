using System;
using UnityEngine;
using UnityEngine.UI;

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

    public Transform GetCoreTransform() { return coreData.coreTransform; }

    private void Awake()
    {
        Instance = this;
        coreData.curlife = coreData.maxLife;
        UpdateLifeBar();
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

    void GameOver()
    {
        Debug.Log("GameOver");
    }

    void UpdateLifeBar()
    {
        coreData.lifeBar.fillAmount = coreData.curlife / coreData.maxLife;
    }
}