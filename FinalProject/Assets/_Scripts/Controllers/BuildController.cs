using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildController : MonoBehaviour
{
    public static BuildController Instance;

    float buildOffsetY = 2.1f;
    [SerializeField] RaycastHit hit;
    [SerializeField] Ray ray;

    [SerializeField] GameObject[] prefabTower;
    GameObject draggableTower;
    BaseTower tempTower;
    Tower tempCombatTower;

    [Header("Build Check")]
    [SerializeField] LayerMask towersLayer;
    [SerializeField] LayerMask surfaceLayer;
    [SerializeField] float checkRadius = 2f;

    private void Awake()
    {
        Instance = this;
    }

    public bool IsBuilding()
    {
        return draggableTower != null;
    }

    public void CreateTower(int index)
    {
        if (draggableTower != null)
        {
            CancelCurrentBuild();
        }

        int cost = BuildUIController.Instance.GetTowerCost(index);

        if (GameManager.Instance.GetGold() < cost)
        {
            SoundManager.Instance.PlaySFX(SoundManager.SFXType.CannotClick);
            return;
        }

        SoundManager.Instance.PlaySFX(SoundManager.SFXType.CanClick);
        GameObject tempTowerObj = Instantiate(prefabTower[index], hit.point, Quaternion.identity);
        draggableTower = tempTowerObj;
        tempTower = tempTowerObj.GetComponent<BaseTower>();
        tempCombatTower = tempTowerObj.GetComponent<Tower>();

        BuildUIController.Instance.ShowCancelButton(true);
    }

    public void CancelCurrentBuild()
    {
        if (draggableTower == null)
        {
            return;
        }

        Destroy(draggableTower);
        draggableTower = null;
        tempTower = null;
        tempCombatTower = null;

        BuildUIController.Instance.ShowCancelButton(false);
    }

    Vector3 SnapToGrid(Vector3 towerPos)
    {
        return new Vector3(
            MathF.Round(towerPos.x),
            towerPos.y,
            Mathf.Round(towerPos.z)
        );
    }

    bool IsSpotOccupied(Vector3 position)
    {
        Collider[] hits = Physics.OverlapSphere(position, checkRadius, towersLayer);

        for (int i = 0; i < hits.Length; i++)
        {
            if (draggableTower != null && hits[i].transform.root.gameObject == draggableTower)
            {
                continue;
            }

            return true;
        }

        return false;
    }

    void Update()
    {
        if (draggableTower == null)
        {
            return;
        }

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, surfaceLayer))
        {
            Vector3 snappedPos = SnapToGrid(hit.point);
            draggableTower.transform.position = snappedPos;

            bool validHeight = hit.point.y > buildOffsetY;
            bool occupied = IsSpotOccupied(snappedPos);
            
            if (Input.GetMouseButtonDown(1))
            {
                CancelCurrentBuild();
                return;
            }

            if (validHeight && !occupied)
            {
                tempTower.Buildable();

                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    int cost = BuildUIController.Instance.GetTowerCost((int)BuildUIController.Instance.selectedTower);

                    if (!GameManager.Instance.SpendGold(cost))
                    {
                        return;
                    }

                    tempTower.Build();
                    tempCombatTower.BuildTower();

                    draggableTower = null;
                    tempTower = null;
                    tempCombatTower = null;

                    BuildUIController.Instance.ShowCancelButton(false);
                    return;
                }
            }
            else
            {
                tempTower.NonBuildable();

                if (Input.GetMouseButtonDown(0))
                {
                    SoundManager.Instance.PlaySFX(SoundManager.SFXType.CannotClick);
                }
            }
        }
    }
}
