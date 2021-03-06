﻿using UnityEngine;

public class RangeTower : Tower
{
    [Header("Audio")]
    [SerializeField]
    private AudioClip arrowShotClip;

    protected override void Start()
    {
        base.Start();
        InvokeRepeating("HitFirstEnemy", 0, TimeTillTick());
    }

    private void HitFirstEnemy()
    {
        Enemies.RemoveAll(PhysicalObjects => PhysicalObjects == null);
        if (Enemies.Count > 0)
        {
            PhysicalObject target = Enemies[0];
            foreach (PhysicalObject enemy in Enemies)
            {
                if (!enemy.IsDead()
                 && Vector3.Distance(this.transform.position, target.transform.position) > MinRange)
                {
                    target = enemy;
                }
            }
            myBase.ShootArrow(gameObject, new Vector3(0, 1, 0), target.gameObject, AttackPower, arrowShotClip);
        }
    }

    protected override float GetAttackPowerUpgradeValue()
    {
        return InitValues.upradeRangeTowerAttackPower;
    }

    protected override float GetAttackSpeedUpgradeValue()
    {
        return InitValues.upradeRangeTowerAttackSpeed;
    }

    protected override float GetHealthUpgradeValue()
    {
        return InitValues.upradeRangeTowerHealth;
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        HudDIncrementPhysicalObjectAmountByOne(PhysicalObjectType.RangeTower);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        HudDecrementPhysicalObjectAmountByOne(PhysicalObjectType.RangeTower);
    }

}
