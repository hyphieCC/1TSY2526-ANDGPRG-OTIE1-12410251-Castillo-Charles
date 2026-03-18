using UnityEngine;

public class BossGroundEnemy : GroundEnemy
{
    protected override void Start()
    {
        maxHp = 30f;
        coreDamage = 3;
        goldReward = 10;

        base.Start();
    }
}