﻿using UnityEngine;

public class RangeMinion : Minion
{

    [Header("Audio")]
    [SerializeField]
    private AudioClip arrowShotClip;

    private float timeSinceHit;

    protected override void Start()
    {
        base.Start();
        this.transform.Find("Type").gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
    }

    protected override void Update()
    {
        base.Update();
        
        if (IsDead()) return;

        timeSinceHit += Time.deltaTime;
        if (timeSinceHit >= TimeTillTick())
        {
            HitFirstEnemy();
            timeSinceHit = 0;
        }
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
                    break;
                }
                //Debug.DrawLine(this.transform.position, enemy.transform.position, this.transform.Find("Base").gameObject.GetComponent<Renderer>().material.GetColor("_Color"), 2f);
            }
            myBase.ShootArrow(gameObject.gameObject, new Vector3(0, 1, 0), target.gameObject, AttackPower, arrowShotClip);
        }
    }
    
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        HudDIncrementPhysicalObjectAmountByOne(PhysicalObjectType.RangeMinion);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        HudDecrementPhysicalObjectAmountByOne(PhysicalObjectType.RangeMinion);
    }
}
