using UnityEngine;

public class BossGroundEnemy : GroundEnemy
{
    protected override void Awake()
    {
        base.Awake();

        maxHp = 30f;
        coreDamage = 5;
        goldReward = 50;
    }
}