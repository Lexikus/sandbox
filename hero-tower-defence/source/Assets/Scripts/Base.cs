using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Base : AiObject
{
    [Header("Prefabs")]
    [SerializeField]
    private GameObject heroPrefab;
    [SerializeField] private GameObject meleeMinionPrefab;
    [SerializeField] private GameObject rangeMinionPrefab;
    [SerializeField] private GameObject meleeTowerPrefab;
    [SerializeField] private GameObject rangeTowerPrefab;
    [SerializeField] private GameObject arrowPrefab;

    [Header("Audio")]
    [SerializeField]
    private AudioClip arrowShotClip;

    public GameObject hero;

    private PhysicalAttributes baseAttributes;
    private PhysicalAttributes meleeMinionAttributes;
    private PhysicalAttributes rangeMinionAttributes;
    private PhysicalAttributes meleeTowerAttributes;
    private PhysicalAttributes rangeTowerAttributes;
    private PhysicalAttributes heroAttributes;

    [SyncVar]
    private int gold;

    public int Gold
    {
        get { return gold; }
        set
        {
            if (!hasAuthority) return;
            CmdSetGold(value);
            if (HUD.Instance != null) HUD.Instance.SetGold(gold);
        }
    }

    [Command]
    private void CmdSetGold(int value)
    {
        try
        {
            gold = value;

        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
            RpcServerLog(e.ToString());
        }
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        SetBaseTag();
        InitValues.InitBase(ref baseAttributes);
        PhysicalAttributes = baseAttributes;
        Gold = InitValues.startGold;
        InvokeRepeating("SpawnWave", 1, PhysicalObject.TimeTillTick(0.05f));

        InitBaseDefence();

        if (!hasAuthority) return;
        CmdSpawnHero(tag);
    }

    private void Awake()
    {
        HUD.OnHUDIsReady += RegisterEvents;
    }

    protected override void Start()
    {
        base.Start();
        InitValues.InitMeleeMinion(ref meleeMinionAttributes);
        InitValues.InitRangeMinion(ref rangeMinionAttributes);
        InitValues.InitMeleeTower(ref meleeTowerAttributes);
        InitValues.InitRangeTower(ref rangeTowerAttributes);
        InitValues.InitHero(ref heroAttributes);

        SetBaseTag();
        DefineBases();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void InitBaseDefence()
    {
        InvokeRepeating("HitAllEnemies", 1, TimeTillTick());
    }

    private void HitAllEnemies()
    {
        Enemies.RemoveAll(PhysicalObjects => PhysicalObjects == null);
        foreach (PhysicalObject enemy in Enemies)
        {
            ShootArrow(this.gameObject, new Vector3(0, 1, 0), enemy.gameObject, AttackSpeed, arrowShotClip);
        }
    }

    private void SetBaseTag()
    {
        GameObject sp1 = GameObject.Find("Spawn1");
        GameObject sp2 = GameObject.Find("Spawn2");

        if (transform.position == sp1.transform.position)
        {
            tag = "Alpha";
        }
        else if (transform.position == sp2.transform.position)
        {
            tag = "Omega";
        }
        else
        {
            tag = "Untagged";
        }
    }

    public void HeroRespawn()
    {
        StartCoroutine("HeroRespawnCount");
    }

    public IEnumerator HeroRespawnCount()
    {
        if (!isLocalPlayer) yield break;
        if (!hasAuthority) yield break;
        const int RESPAWNTIME = 25;
        for (int i = 0; i < RESPAWNTIME; i++)
        {
            if (HUD.Instance != null)
            {
                HUD.Instance.ShowWarningNotification("Respawn in " + (RESPAWNTIME - i) + " ...");
            }
            yield return new WaitForSeconds(1);
        }
        CmdSpawnHero(tag);
        yield return null;
    }

    private void DefineBases()
    {
        if (!isLocalPlayer)
        {
            enemyBase = gameObject.GetComponent<Base>();
        }
        if (isLocalPlayer)
        {
            myBase = gameObject.GetComponent<Base>();
        }
    }

    private void SpawnWave()
    {
        if (!isLocalPlayer) return;
        if (enemyBase == null) return;
        if (meleeMinionPrefab == null) return;
        if (rangeMinionPrefab == null) return;
        if (!hasAuthority) return;

        const int WAVECOUNT = 1;
        for (int i = 0; i < WAVECOUNT; i++)
        {
            CmdSpawnPhysicalObject(transform.position, PhysicalObjectType.MeleeMinion, tag, meleeMinionAttributes);
        }
        for (int i = 0; i < WAVECOUNT; i++)
        {
            CmdSpawnPhysicalObject(transform.position, PhysicalObjectType.RangeMinion, tag, rangeMinionAttributes);
        }
    }

    [Command]
    private void CmdSpawnHero(string tag)
    {
        try
        {
            GameObject hero = Instantiate(heroPrefab, this.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            hero.tag = tag;
            NetworkServer.SpawnWithClientAuthority(hero, connectionToClient);
            RpcSetHero(hero, tag);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
            RpcServerLog(e.ToString());
        }
    }

    [ClientRpc]
    private void RpcSetHero(GameObject hero, string tag)
    {
        if (hero == null) return;
        this.hero = hero;
        this.hero.tag = tag;
        if (HUD.Instance == null) return;
        if (this.hero.GetComponent<Hero>() == null) return;
        HUD.Instance.SetHeroStats(this.hero.GetComponent<Hero>().AttackPower, this.hero.GetComponent<Hero>().Armor, this.hero.GetComponent<Hero>().AttackSpeed);
        
    }

    [Command]
    private void CmdSpawnPhysicalObject(Vector3 position, PhysicalObjectType type, string tag, PhysicalAttributes attributes)
    {
        try
        {
            GameObject gameObject = null;
            switch (type)
            {
                case PhysicalObjectType.MeleeMinion:
                    gameObject = Instantiate(meleeMinionPrefab, position + new Vector3(0, 1, 0), Quaternion.identity);
                    break;
                case PhysicalObjectType.RangeMinion:

                    gameObject = Instantiate(rangeMinionPrefab, position + new Vector3(0, 1, 0), Quaternion.identity);
                    break;
                case PhysicalObjectType.MeleeTower:

                    gameObject = Instantiate(meleeTowerPrefab, position + new Vector3(0, 1, 0), Quaternion.identity);
                    break;
                case PhysicalObjectType.RangeTower:

                    gameObject = Instantiate(rangeTowerPrefab, position + new Vector3(0, 1, 0), Quaternion.identity);
                    break;
                default:
                    return;
            }
            if (gameObject == null)
            {
                RpcServerLog(type.ToString() + " could not get Instantiate");
                return;
            }
            gameObject.tag = tag;
            if (gameObject.GetComponent<PhysicalObject>() != null)
            {
                gameObject.GetComponent<PhysicalObject>().PhysicalAttributes = attributes;
            }
            else
            {
                RpcServerLog(gameObject.name + " has no PhysicalObject");
            }
            NetworkServer.SpawnWithClientAuthority(gameObject, connectionToClient);
            RpcSetPhysicalObjectStats(gameObject, tag, attributes);

        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
            RpcServerLog(e.ToString());
        }
    }

    [ClientRpc]
    private void RpcSetPhysicalObjectStats(GameObject gameObject, string tag, PhysicalAttributes attributes)
    {
        if (gameObject == null) return;
        gameObject.tag = tag;
        if (gameObject.GetComponent<PhysicalObject>() != null)
        {
            gameObject.GetComponent<PhysicalObject>().PhysicalAttributes = attributes;
        }
        else
        {
            Debug.LogWarning(gameObject.name + " has no PhysicalObject");
        }
    }

    public void ShootArrow(GameObject source, Vector3 sourceDelta, GameObject target, float attackPower, AudioClip shootAudio)
    {
        if (!isLocalPlayer) return;
        if (!hasAuthority) return;
        if (this.gameObject == null) return;
        InitValues.PlayLowPrioAudioSource(gameObject, shootAudio, 0, 0);
        CmdShootArrow(source, sourceDelta, target, attackPower);
    }

    [Command]
    private void CmdShootArrow(GameObject source, Vector3 sourceDelta, GameObject target, float attackPower)
    {
        try
        {
            if (source == null) return;
            if (target == null) return;

            GameObject newArrow = Instantiate(arrowPrefab, source.transform.position + sourceDelta, Quaternion.identity);
            newArrow.tag = tag;
            NetworkServer.SpawnWithClientAuthority(newArrow, connectionToClient);

            newArrow.GetComponent<Arrow>().SetDestination(target, source, attackPower);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
            RpcServerLog(e.ToString());
        }
    }

    private void RegisterEvents()
    {
        if (HUD.Instance == null) return;
        if (!isLocalPlayer) return;
        HUD.Instance.OnPlaceMeleeTower += PlaceMeleeTower;
        HUD.Instance.OnPlaceRangeTower += PlaceRangeTower;
        HUD.Instance.OnSpawnMeleeMinion += SpawnMeleeMinion;
        HUD.Instance.OnSpawnRangeMinion += SpawnRangeMinion;
        HUD.Instance.OnUpgradeMeleeMinionAttackPower += UpgradeMeleeMinionAttackPower;
        HUD.Instance.OnUpgradeMeleeMinionHealth += UpgradeMeleeMinionHealth;
        HUD.Instance.OnUpgradeRangeMinionAttackPower += UpgradeRangeMinionAttackPower;
        HUD.Instance.OnUpgradeRangeMinionHealth += UpgradeRangeMinionHealth;
        HUD.Instance.OnUpgradeHeroAttackPower += UpgradeHeroAttackPower;
        HUD.Instance.OnUpgradeHeroArmor += UpgradeHeroArmor;
        HUD.Instance.OnUpgradeHeroAttackSpeed += UpgradeHeroAttackSpeed;
        HUD.Instance.OnUpgradeHeroHealth += UpgradeHeroHealth;

        HUD.Instance.SetGold(Gold);
        lifeBar = HUD.Instance.heroLifebarSlider;
        if (lifeBar != null)
        {
            lifeBar.maxValue = MaxHealth;
            lifeBar.value = CurrentHealth;
        }
        
        if (hero.GetComponent<Hero>() == null) return;
        HUD.Instance.SetHeroStats(this.hero.GetComponent<Hero>().AttackPower, this.hero.GetComponent<Hero>().Armor, this.hero.GetComponent<Hero>().AttackSpeed);
    }

    private void SpawnMeleeMinion()
    {
        if (meleeMinionPrefab == null) return;
        if (hero == null) return;
        if (!IsOnYourSide(hero)) return;
        if (!TryToPay(meleeMinionAttributes.spawnGoldCosts)) return;
        if (!hasAuthority) return;
        CmdSpawnPhysicalObject(hero.transform.position, PhysicalObjectType.MeleeMinion, tag, meleeMinionAttributes);
    }

    private void SpawnRangeMinion()
    {
        if (rangeMinionPrefab == null) return;
        if (hero == null) return;
        if (!IsOnYourSide(hero)) return;
        if (!TryToPay(rangeMinionAttributes.spawnGoldCosts)) return;
        if (!hasAuthority) return;
        CmdSpawnPhysicalObject(hero.transform.position, PhysicalObjectType.RangeMinion, tag, rangeMinionAttributes);
    }

    private void PlaceMeleeTower()
    {
        if (meleeTowerPrefab == null) return;
        if (hero == null) return;
        if (!hasAuthority) return;
        if (!IsOnYourSide(hero)) return;
        if (TowerInRange(hero.transform.position, GetTowerRadius(meleeTowerPrefab) * 2))
        {
            if (HUD.Instance != null)
            {
                HUD.Instance.ShowWarningNotification("Zu nah an einem anderen Tower");
            }
            return;
        }
        if (!TryToPay(meleeTowerAttributes.spawnGoldCosts)) return;
        CmdSpawnPhysicalObject(hero.transform.position, PhysicalObjectType.MeleeTower, tag, meleeTowerAttributes);
    }

    private void PlaceRangeTower()
    {
        if (rangeTowerPrefab == null) return;
        if (hero == null) return;
        if (!hasAuthority) return;
        if (!IsOnYourSide(hero)) return;
        if (TowerInRange(hero.transform.position, GetTowerRadius(rangeTowerPrefab) * 2))
        {
            if (HUD.Instance != null)
            {
                HUD.Instance.ShowWarningNotification("Zu nah an einem anderen Towen");
            }
            return;
        }
        if (!TryToPay(rangeTowerAttributes.spawnGoldCosts)) return;
        CmdSpawnPhysicalObject(hero.transform.position, PhysicalObjectType.RangeTower, tag, rangeTowerAttributes);
    }

    private void UpgradeMeleeMinionAttackPower()
    {
        if (!TryToPay(meleeMinionAttributes.upgradeGoldCosts)) return;
        meleeMinionAttributes.attackPower += InitValues.upgradeMeleeMinionAttackPower;
        foreach (MeleeMinion meleeMinion in FindObjectsOfType<MeleeMinion>())
        {
            if (meleeMinion.tag != this.tag) continue;
            meleeMinion.AttackPower += InitValues.upgradeMeleeMinionAttackPower;
        }
    }

    private void UpgradeMeleeMinionHealth()
    {
        if (!TryToPay(meleeMinionAttributes.upgradeGoldCosts)) return;
        meleeMinionAttributes.maxHealth += InitValues.upgradeMeleeMinionHealth;
        foreach (MeleeMinion meleeMinion in FindObjectsOfType<MeleeMinion>())
        {
            if (meleeMinion.tag != this.tag) continue;
            meleeMinion.MaxHealth += InitValues.upgradeMeleeMinionHealth;
        }
    }

    private void UpgradeRangeMinionAttackPower()
    {
        if (!TryToPay(rangeMinionAttributes.upgradeGoldCosts)) return;
        rangeMinionAttributes.attackPower += InitValues.upgradeRangeMinionAttackPower;
        foreach (RangeMinion rangeMinon in FindObjectsOfType<RangeMinion>())
        {
            if (rangeMinon.tag != this.tag) continue;
            rangeMinon.AttackPower += InitValues.upgradeRangeMinionAttackPower;
        }
    }

    private void UpgradeRangeMinionHealth()
    {
        if (!TryToPay(rangeMinionAttributes.upgradeGoldCosts)) return;
        rangeMinionAttributes.maxHealth += InitValues.upgradeRangeMinionHealth;
        foreach (RangeMinion rangeMinon in FindObjectsOfType<RangeMinion>())
        {
            if (rangeMinon.tag != this.tag) continue;
            rangeMinon.MaxHealth += InitValues.upgradeRangeMinionHealth;
        }
    }

    private void UpgradeHeroAttackPower()
    {
        if (!TryToPay(heroAttributes.upgradeGoldCosts)) return;
        heroAttributes.attackPower += InitValues.upgradeHeroAttackPower;
        foreach (Hero hero in FindObjectsOfType<Hero>())
        {
            if (hero.tag != this.tag) continue;
            hero.AttackPower += InitValues.upgradeHeroAttackPower;
            HUD.Instance.SetHeroStats(hero.AttackPower, hero.Armor, hero.AttackSpeed);
        }
    }

    private void UpgradeHeroArmor()
    {
        if (!TryToPay(heroAttributes.upgradeGoldCosts)) return;
        heroAttributes.armor += InitValues.upgradeHeroArmor;
        foreach (Hero hero in FindObjectsOfType<Hero>())
        {
            if (hero.tag != this.tag) continue;
            hero.Armor += InitValues.upgradeHeroArmor;
            HUD.Instance.SetHeroStats(hero.AttackPower, hero.Armor, hero.AttackSpeed);
        }
    }

    private void UpgradeHeroAttackSpeed()
    {
        if (!TryToPay(heroAttributes.upgradeGoldCosts)) return;
        heroAttributes.attackSpeed += InitValues.upgradeHeroAttackSpeed;
        foreach (Hero hero in FindObjectsOfType<Hero>())
        {
            if (hero.tag != this.tag) continue;
            hero.AttackSpeed += InitValues.upgradeHeroAttackSpeed;
            HUD.Instance.SetHeroStats(hero.AttackPower, hero.Armor, hero.AttackSpeed);
        }
    }

    private void UpgradeHeroHealth()
    {
        if (!TryToPay(heroAttributes.upgradeGoldCosts)) return;
        heroAttributes.maxHealth += InitValues.upgradeHeroHealth;
        foreach (Hero hero in FindObjectsOfType<Hero>())
        {
            if (hero.tag != this.tag) continue;
            hero.MaxHealth += InitValues.upgradeHeroHealth;
        }
    }

    private bool IsOnYourSide(GameObject gameObject)
    {
        foreach (Collider item in Physics.OverlapBox(transform.position, new Vector3(100, 1, 35), Quaternion.Euler(0, 45, 0)))
        {
            if (item.tag != tag) continue;
            if (item.name == gameObject.name)
            {
                return true;
            }
        }
        if (HUD.Instance != null)
        {
            HUD.Instance.ShowWarningNotification("Nicht in deiner Spielhälfte");
        }
        return false;
    }

    private bool TowerInRange(Vector3 position, float radius)
    {
        foreach (Collider item in Physics.OverlapSphere(position, radius))
        {
            if (item.tag != tag) continue;
            if (item.name == "meleeTower(Clone)") return true;
            if (item.name == "RangeTower(Clone)") return true;
        }
        return false;
    }

    private float GetTowerRadius(GameObject gameObject)
    {
        float returnvalue = 0;
        foreach (CapsuleCollider item in gameObject.GetComponentsInChildren<CapsuleCollider>(true))
        {
            if (item.isTrigger) continue;
            returnvalue = item.radius;
        }
        return returnvalue;
    }

    public bool TryToPay(int cost)
    {
        if (Gold >= cost)
        {
            Gold -= cost;
            return true;
        }
        else
        {
            if (HUD.Instance != null)
            {
                HUD.Instance.ShowWarningNotification(HUD.defaultWarningMessage);
            }
            return false;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (HUD.Instance == null) return;
        if (!isLocalPlayer)
        {
            HUD.Instance.ShowWinLost(true);
        }
        if (isLocalPlayer)
        {
            HUD.Instance.ShowWinLost(false);
        }
    }

    private void OnValidate()
    {
        ToolBox.CheckSerializeField(ref heroPrefab);
        ToolBox.CheckSerializeField(ref meleeMinionPrefab);
        ToolBox.CheckSerializeField(ref rangeMinionPrefab);
        ToolBox.CheckSerializeField(ref meleeTowerPrefab);
        ToolBox.CheckSerializeField(ref rangeTowerPrefab);
        ToolBox.CheckSerializeField(ref arrowPrefab);
    }
}
