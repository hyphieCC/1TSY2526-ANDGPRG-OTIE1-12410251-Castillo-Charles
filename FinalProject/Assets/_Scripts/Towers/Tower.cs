using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] protected float range = 6f;
    [SerializeField] protected float attackRate = 1f;

    [Header("Targeting")]
    [SerializeField] protected bool canTargetGround = true;
    [SerializeField] protected bool canTargetFlying = true;

    [Header("Visuals")]
    [SerializeField] protected Transform pivot;
    [SerializeField] protected float rotateSpeed = 8f;

    [Header("Upgrading")]
    [SerializeField] protected int upgradeALevel = 1;
    [SerializeField] protected int upgradeBLevel = 1;
    [SerializeField] protected int maxUpgradeLevel = 3;
    [SerializeField] protected int upgradeABaseCost = 100;
    [SerializeField] protected int upgradeAIncreasePerLevel = 150;
    [SerializeField] protected int upgradeBBaseCost = 100;
    [SerializeField] protected int upgradeBIncreasePerLevel = 150;

    protected float attackTimer;
    protected bool isBuilt = false;
    protected Enemy currentTarget;

    protected virtual void Update()
    {
        if (!isBuilt)
        {
            return;
        }

        currentTarget = GetTarget();

        if (currentTarget != null)
        {
            RotateTowardsTarget();
        }

        attackTimer += Time.deltaTime;

        if (attackTimer >= attackRate)
        {
            if (currentTarget != null)
            {
                Attack(currentTarget);
                attackTimer = 0f;
            }
        }
    }

    public void BuildTower()
    {
        isBuilt = true;
    }

    private void OnMouseDown()
    {
        if (!isBuilt)
        {
            return;
        }

        if (BuildController.Instance != null && BuildController.Instance.IsBuilding())
        {
            BuildController.Instance.CancelCurrentBuild();
        }

        TowerSelectionController.Instance.SelectTower(this);
    }

    public virtual string GetTowerName()
    {
        return "SELECTED TOWER";
    }

    public virtual string GetStatsText()
    {
        return "Level: " + GetTotalLevel() +
            "\nRange: " + range +
            "\nAttack Rate: " + attackRate +
            "\nA Cost: " + GetUpgradeCostA() +
            "\nB Cost: " + GetUpgradeCostB();
    }

    public int GetTotalLevel()
    {
        return upgradeALevel + upgradeBLevel - 1;
    }

    public int GetUpgradeALevel()
    {
        return upgradeALevel;
    }

    public int GetUpgradeBLevel()
    {
        return upgradeBLevel;
    }

    public int GetUpgradeCostA()
    {
        return upgradeABaseCost + (upgradeALevel - 1) * upgradeAIncreasePerLevel;
    }

    public int GetUpgradeCostB()
    {
        return upgradeBBaseCost + (upgradeBLevel - 1) * upgradeBIncreasePerLevel;
    }

    public bool CanUpgradeA()
    {
        return upgradeALevel < maxUpgradeLevel;
    }

    public bool CanUpgradeB()
    {
        return upgradeBLevel < maxUpgradeLevel;
    }

    public virtual string GetUpgradeOptionAText()
    {
        return "Upgrade A";
    }

    public virtual string GetUpgradeOptionBText()
    {
        return "Upgrade B";
    }

    public void ApplyUpgradeChoice(int choiceIndex)
    {
        if (choiceIndex == 0)
        {
            if (!CanUpgradeA())
            {
                return;
            }

            if (!GameManager.Instance.SpendGold(GetUpgradeCostA()))
            {
                return;
            }

            ApplyUpgradeA();
            upgradeALevel++;
        }
        else if (choiceIndex == 1)
        {
            if (!CanUpgradeB())
            {
                return;
            }

            if (!GameManager.Instance.SpendGold(GetUpgradeCostB()))
            {
                return;
            }

            ApplyUpgradeB();
            upgradeBLevel++;
        }
    }

    protected virtual void ApplyUpgradeA()
    {
    }

    protected virtual void ApplyUpgradeB()
    {
    }

    protected Enemy GetTarget()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        Enemy lowestHpTarget = null;
        float lowestHp = float.MaxValue;

        for (int i = 0; i < enemies.Length; i++)
        {
            if (!CanTarget(enemies[i]))
            {
                continue;
            }

            float distance = Vector3.Distance(transform.position, enemies[i].transform.position);

            if (distance > range)
            {
                continue;
            }

            if (enemies[i].GetCurrentHp() < lowestHp)
            {
                lowestHp = enemies[i].GetCurrentHp();
                lowestHpTarget = enemies[i];
            }
        }

        return lowestHpTarget;
    }

    protected bool CanTarget(Enemy enemy)
    {
        if (enemy.GetEnemyType() == EnemyType.Ground && canTargetGround)
            return true;

        if (enemy.GetEnemyType() == EnemyType.Flying && canTargetFlying)
            return true;

        return false;
    }

    void RotateTowardsTarget()
    {
        Vector3 direction = currentTarget.transform.position - pivot.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        pivot.rotation = Quaternion.Slerp(pivot.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    protected abstract void Attack(Enemy target);

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, range);
    //}
}