using UnityEngine;

public class TowerSelectionController : MonoBehaviour
{
    public static TowerSelectionController Instance;

    Tower selectedTower;

    private void Awake()
    {
        Instance = this;
    }

    public void SelectTower(Tower tower)
    {
        selectedTower = tower;
        UpgradeUIController.Instance.ShowUpgradeUI(tower);
    }

    public Tower GetSelectedTower()
    {
        return selectedTower;
    }

    public void DeselectTower()
    {
        selectedTower = null;
        UpgradeUIController.Instance.HideUpgradeUI();
    }
}
