using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildUIController : MonoBehaviour
{
    public static BuildUIController Instance;

    [SerializeField] BuildController buildController;

    public enum TowerType
    {
        Arrow,
        Cannon,
        Ice,
        Fire
    }

    public TowerType selectedTower;

    [Header("UI")]
    [SerializeField] TMP_Text towerName;
    [SerializeField] TMP_Text towerText;
    [SerializeField] GameObject cancelBuildButton;
    [SerializeField] TMP_Text arrowCostText;
    [SerializeField] TMP_Text cannonCostText;
    [SerializeField] TMP_Text iceCostText;
    [SerializeField] TMP_Text fireCostText;

    [Header("Tower Costs")]
    [SerializeField] int arrowCost = 50;
    [SerializeField] int cannonCost = 150;
    [SerializeField] int iceCost = 120;
    [SerializeField] int fireCost = 150;

    private void Awake()
    {
        Instance = this;
        ShowCancelButton(false);

        arrowCostText.text = "Cost: " + arrowCost;
        cannonCostText.text = "Cost: " + cannonCost;
        iceCostText.text = "Cost: " + iceCost;
        fireCostText.text = "Cost: " + fireCost;
    }

    public void SelectTower(int towerIndex)
    {
        selectedTower = (TowerType)towerIndex;

        UpdateDescription(selectedTower);
        buildController.CreateTower(towerIndex);
    }

    public int GetTowerCost(int towerIndex)
    {
        switch ((TowerType)towerIndex)
        {
            case TowerType.Arrow:
                return arrowCost;
            case TowerType.Cannon:
                return cannonCost;
            case TowerType.Ice:
                return iceCost;
            case TowerType.Fire:
                return fireCost;
        }

        return 0;
    }

    public void ShowCancelButton(bool show)
    {
        cancelBuildButton.SetActive(show);
    }

    public void CancelBuild()
    {
        SoundManager.Instance.PlaySFX(SoundManager.SFXType.CanClick);
        buildController.CancelCurrentBuild();
    }

    void UpdateDescription(TowerType type)
    {
        switch (type)
        {
            case TowerType.Arrow:
                towerName.text = "Arrow Tower";
                towerText.text = "Fast attack.\nCan hit flying.\nCost: " + arrowCost;
                break;

            case TowerType.Cannon:
                towerName.text = "Cannon Tower";
                towerText.text = "Splash damage.\nGround only.\nCost: " + cannonCost;
                break;

            case TowerType.Ice:
                towerName.text = "Ice Tower";
                towerText.text = "Slows enemies.\nCost: " + iceCost;
                break;

            case TowerType.Fire:
                towerName.text = "Fire Tower";
                towerText.text = "Damage over time.\nCost: " + fireCost;
                break;
        }
    }
}
