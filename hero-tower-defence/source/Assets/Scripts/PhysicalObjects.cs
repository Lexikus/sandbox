using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public abstract class PhysicalObject : NetworkBehaviour
{
    [SyncVar(hook = "OnChangePhysicalAttributes")]
    private PhysicalAttributes physicalAttributes = new PhysicalAttributes(666, 0, 1, 1, 0, 2, 20, 10, 5, 10, 1);
    [SyncVar(hook = "OnChangeCurrentHealth")]
    private float currentHealth = 100;

    private List<PhysicalObject> enemies = new List<PhysicalObject>();

    protected static Base enemyBase = null;
    protected static Base myBase = null;

    protected Slider lifeBar;

    protected const string moveId = "Move";
    protected const string isDeadId = "IsDead";
    protected const string isAttackingId = "IsAttacking";
    protected const string attackingSpeedId = "AttackingSpeed";

    public PhysicalAttributes PhysicalAttributes
    {
        private get { return physicalAttributes; }
        set
        {
            if (!hasAuthority) return;
            CmdSetPhysicalAttributes(value);
        }
    }

    [Command]
    private void CmdSetPhysicalAttributes(PhysicalAttributes value)
    {
        try
        {
            physicalAttributes = value;
            currentHealth = physicalAttributes.maxHealth;
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
            RpcServerLog(e.ToString());
        }
    }

    private void OnChangePhysicalAttributes(PhysicalAttributes value)
    {
        if (GetComponent<SphereCollider>() != null)
        {
            this.GetComponent<SphereCollider>().radius = value.maxRange;

        }
        if (lifeBar != null)
        {
            lifeBar.maxValue = value.maxHealth;
            lifeBar.value = CurrentHealth;
        }
    }

    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            if (!hasAuthority) return;
            CmdSetCurrentHealth(value);
        }
    }

    [Command]
    private void CmdSetCurrentHealth(float value)
    {
        try
        {
            currentHealth = value;
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
            RpcServerLog(e.ToString());
        }
    }

    private void OnChangeCurrentHealth(float value)
    {
        if (lifeBar == null) return;
        lifeBar.value = value;
    }

    public float MaxHealth
    {
        get { return physicalAttributes.maxHealth; }
        set
        {
            if (!hasAuthority) return;
            CmdSetPhysicalAttributes(new PhysicalAttributes(value, physicalAttributes.armor, physicalAttributes.attackPower, physicalAttributes.attackSpeed, physicalAttributes.minRange, physicalAttributes.maxRange, physicalAttributes.destroyGoldValue, physicalAttributes.spawnGoldCosts, physicalAttributes.upgradeGoldCosts, physicalAttributes.movemenetSpeed, physicalAttributes.despawnTime));
        }
    }
    
    public float Armor
    {
        get { return physicalAttributes.armor; }
        set
        {
            if (!hasAuthority) return;
            CmdSetPhysicalAttributes(new PhysicalAttributes(physicalAttributes.maxHealth, value, physicalAttributes.attackPower, physicalAttributes.attackSpeed, physicalAttributes.minRange, physicalAttributes.maxRange, physicalAttributes.destroyGoldValue, physicalAttributes.spawnGoldCosts, physicalAttributes.upgradeGoldCosts, physicalAttributes.movemenetSpeed, physicalAttributes.despawnTime));
        }
    }
    
    public float AttackPower
    {
        get { return physicalAttributes.attackPower; }
        set
        {
            if (!hasAuthority) return;
            CmdSetPhysicalAttributes(new PhysicalAttributes(physicalAttributes.maxHealth, physicalAttributes.armor, value, physicalAttributes.attackSpeed, physicalAttributes.minRange, physicalAttributes.maxRange, physicalAttributes.destroyGoldValue, physicalAttributes.spawnGoldCosts, physicalAttributes.upgradeGoldCosts, physicalAttributes.movemenetSpeed, physicalAttributes.despawnTime));
        }
    }
    public float AttackSpeed
    {
        get { return physicalAttributes.attackSpeed; }
        set
        {
            if (!hasAuthority) return;
            CmdSetPhysicalAttributes(new PhysicalAttributes(physicalAttributes.maxHealth, physicalAttributes.armor, physicalAttributes.attackPower, value, physicalAttributes.minRange, physicalAttributes.maxRange, physicalAttributes.destroyGoldValue, physicalAttributes.spawnGoldCosts, physicalAttributes.upgradeGoldCosts, physicalAttributes.movemenetSpeed, physicalAttributes.despawnTime));
        }
    }

    public float MinRange
    {
        get { return physicalAttributes.minRange; }
    }

    public float MaxRange
    {
        get { return physicalAttributes.maxRange; }
    }

    public int DestroyGoldValue
    {
        get { return physicalAttributes.destroyGoldValue; }
    }

    public int SpawnGoldCosts
    {
        get { return physicalAttributes.spawnGoldCosts; }
    }

    public int UpgradeGoldCosts
    {
        get { return physicalAttributes.upgradeGoldCosts; }
    }

    public float MovemenetSpeed
    {
        get { return physicalAttributes.movemenetSpeed; }
    }

    public float DespawnTime
    {
        get { return physicalAttributes.despawnTime; }
    }

    public List<PhysicalObject> Enemies
    {
        get { return enemies; }
    }


    [Command]
    public void CmdAddEnemy(GameObject enemy)
    {
        try
        {
            if (enemy == null) return;
            if (enemy.GetComponent<PhysicalObject>() == null) return;
            if (enemy.GetComponent<PhysicalObject>().IsDead()) return;
            enemies.Add(enemy.GetComponent<PhysicalObject>());
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
            RpcServerLog(e.ToString());
        }
    }

    [Command]
    public void CmdRemoveEnemy(GameObject enemy)
    {
        try
        {
            if (enemy == null) return;
            if (enemy.GetComponent<PhysicalObject>() == null) return;
            enemies.Remove(enemy.GetComponent<PhysicalObject>());
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
            RpcServerLog(e.ToString());
        }
    }

    [ClientRpc]
    public void RpcAddEnemy(GameObject enemy)
    {
        if (enemy == null) return;
        if (enemy.GetComponent<PhysicalObject>() == null) return;
        enemies.Add(enemy.GetComponent<PhysicalObject>());
    }

    [ClientRpc]
    public void RpcRemoveEnemy(GameObject enemy)
    {
        if (enemy == null) return;
        if (enemy.GetComponent<PhysicalObject>() == null) return;
        enemies.Add(enemy.GetComponent<PhysicalObject>());
    }

    private void Awake()
    {
        currentHealth = physicalAttributes.maxHealth;
    }

    protected virtual void Start()
    {
        if (isLocalPlayer)
        {
            if (HUD.Instance != null)
            {
                lifeBar = HUD.Instance.heroLifebarSlider;
            }
        }
        else
        {
            if (this.transform.Find("Life") != null)
            {
                lifeBar = this.transform.Find("Life").gameObject.GetComponent<Slider>();
            }
        }
        if (lifeBar != null)
        {
            lifeBar.maxValue = physicalAttributes.maxHealth;
            lifeBar.value = currentHealth;
        }
    }

    protected virtual void Update()
    {
        IsDead();
    }

    public virtual void ApplyDmg(float dmg, GameObject attacker)
    {
        if (!hasAuthority) return;
        if (Armor > 100)
        {
            dmg = dmg - (1 / 100 * Armor);
        }

        CurrentHealth -= Mathf.Abs(dmg);
    }

    public void ApplyHeal(float heal)
    {
        CurrentHealth += Mathf.Abs(heal);
    }

    protected float TimeTillTick()
    {
        return TimeTillTick(physicalAttributes.attackSpeed);
    }

    public static float TimeTillTick(float attackPerSecond)
    {
        return 1 / attackPerSecond;
    }


    public virtual bool IsDead()
    {
        if (CurrentHealth <= 0)
        {
            if (physicalAttributes.despawnTime >= 0)
            {
                Destroy(this.gameObject, physicalAttributes.despawnTime);
            }
            return true;
        }
        return false;
    }

    [ClientRpc]
    public void RpcServerLog(string text)
    {
        Debug.Log(text);
    }

    public virtual void ApplyDmgFromAnimation()
    {
    }

    protected virtual void OnDestroy()
    {
        if (hasAuthority) return;
        if (enemyBase == null) return;
        myBase.Gold += DestroyGoldValue;
    }
}