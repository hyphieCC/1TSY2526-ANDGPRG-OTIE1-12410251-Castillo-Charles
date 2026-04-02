using UnityEngine;

public class BaseTower : MonoBehaviour
{
    [SerializeField] Material towerMat;
    [SerializeField] Renderer[] towerRenderers;

    Material tempTowerMat;

    private void Awake()
    {
        tempTowerMat = new Material(towerMat);

        for (int i = 0; i < towerRenderers.Length; i++)
        {
            towerRenderers[i].material = tempTowerMat;
        }
    }

    public void Buildable()
    {
        tempTowerMat.color = Color.green;
    }

    public void NonBuildable()
    {
        tempTowerMat.color = Color.red;
    }

    public void Build()
    {
        tempTowerMat.color = Color.white;
    }
}