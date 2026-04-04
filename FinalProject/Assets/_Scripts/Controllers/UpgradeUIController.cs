using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIController : MonoBehaviour
{
    public static UpgradeUIController Instance;

    [Header("Information")]
    [SerializeField] GameObject upgradePanel;
    [SerializeField] TMP_Text towerHeaderText;
    [SerializeField] TMP_Text statsText;
    [SerializeField] TMP_Text selectedUpgradeText;
    [SerializeField] TMP_Text upgradeACostText;
    [SerializeField] TMP_Text upgradeBCostText;

    [Header("Buttons")]
    [SerializeField] Button upgradeAButton;
    [SerializeField] Button upgradeBButton;
    [SerializeField] Button confirmButton;
    [SerializeField] TMP_Text upgradeAButtonText;
    [SerializeField] TMP_Text upgradeBButtonText;

    Tower currentTower;

    Image upgradeAImage;
    Image upgradeBImage;

    Color normalColor = Color.white;
    Color selectedColor = new Color(1.2f, 1.2f, 1.2f);
    Color dimColor = new Color(0.75f, 0.75f, 0.75f);

    int selectedUpgradeIndex = -1;

    private void Awake()
    {
        Instance = this;

        upgradePanel.SetActive(false);

        upgradeAImage = upgradeAButton.GetComponent<Image>();
        upgradeBImage = upgradeBButton.GetComponent<Image>();

        confirmButton.interactable = false;
    }

    public void ShowUpgradeUI(Tower tower)
    {
        currentTower = tower;
        upgradePanel.SetActive(true);

        RefreshUI();
        ResetSelection();
    }

    public void HideUpgradeUI()
    {
        currentTower = null;
        upgradePanel.SetActive(false);
    }

    void RefreshUI()
    {
        if (currentTower == null)
        {
            return;
        }

        towerHeaderText.text = currentTower.GetTowerName();
        statsText.text = currentTower.GetStatsText();

        if (currentTower.CanUpgradeA())
        {
            upgradeACostText.gameObject.SetActive(true);
            upgradeACostText.text = "Cost: " + currentTower.GetUpgradeCostA();
        }
        else
        {
            upgradeACostText.gameObject.SetActive(false);
        }

        if (currentTower.CanUpgradeB())
        {
            upgradeBCostText.gameObject.SetActive(true);
            upgradeBCostText.text = "Cost: " + currentTower.GetUpgradeCostB();
        }
        else
        {
            upgradeBCostText.gameObject.SetActive(false);
        }

        upgradeAButtonText.text = currentTower.GetUpgradeOptionAText();
        upgradeBButtonText.text = currentTower.GetUpgradeOptionBText();

        upgradeAButton.interactable = currentTower.CanUpgradeA();
        upgradeBButton.interactable = currentTower.CanUpgradeB();

        if (!currentTower.CanUpgradeA() && !currentTower.CanUpgradeB())
        {
            selectedUpgradeText.text = "All Upgrades Maxed";
            confirmButton.interactable = false;
        }
        else
        {
            selectedUpgradeText.text = "Select an upgrade";
        }
    }

    void ResetSelection()
    {
        selectedUpgradeIndex = -1;

        upgradeAImage.color = normalColor;
        upgradeBImage.color = normalColor;

        confirmButton.interactable = false;
    }

    public void SelectUpgradeA()
    {
        if (currentTower == null || !currentTower.CanUpgradeA())
        {
            return;
        }

        selectedUpgradeIndex = 0;

        upgradeAImage.color = selectedColor;
        upgradeBImage.color = dimColor;

        selectedUpgradeText.text = currentTower.GetUpgradeOptionAText() + "\nCost: " + currentTower.GetUpgradeCostA();
        confirmButton.interactable = true;
    }

    public void SelectUpgradeB()
    {
        if (currentTower == null || !currentTower.CanUpgradeB())
        {
            return;
        }

        selectedUpgradeIndex = 1;

        upgradeBImage.color = selectedColor;
        upgradeAImage.color = dimColor;

        selectedUpgradeText.text = currentTower.GetUpgradeOptionBText() + "\nCost: " + currentTower.GetUpgradeCostB();
        confirmButton.interactable = true;
    }

    public void ConfirmUpgrade()
    {
        if (currentTower == null || selectedUpgradeIndex == -1)
        {
            return;
        }

        currentTower.ApplyUpgradeChoice(selectedUpgradeIndex);

        RefreshUI();
        ResetSelection();
    }
}