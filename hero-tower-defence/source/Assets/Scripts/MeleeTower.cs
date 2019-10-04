using UnityEngine;

public class MeleeTower : Tower
{
    [Header("Audio")]
    [SerializeField]
    private AudioClip hitClip;

    private GameObject blade;

    protected override void Start()
    {
        base.Start();

        blade = this.transform.Find("Blade").gameObject;

        InvokeRepeating("HitAllEnemies", 0, TimeTillTick());
    }

    protected override void Update()
    {
        base.Update();
        blade.transform.Rotate(new Vector3(0, AttackSpeed * 200, 0) * Time.deltaTime);
    }

    private void HitAllEnemies()
    {
        Enemies.RemoveAll(PhysicalObjects => PhysicalObjects == null);
        foreach (PhysicalObject enemy in Enemies)
        {
            QuadrupelSound();
            //Debug.DrawLine(this.transform.position, enemy.transform.position);
            enemy.ApplyDmg(this.AttackPower, this.gameObject);
        }
    }

    private void QuadrupelSound()
    {
        for (int i = 0; i < 4; i++)
        {
            InitValues.PlayLowPrioAudioSource(gameObject, hitClip, i * 0.2f, 0);
        }
    }
    
    protected override float GetAttackPowerUpgradeValue()
    {
        return InitValues.upradeMeleeTowerAttackPower;
    }

    protected override float GetAttackSpeedUpgradeValue()
    {
        return InitValues.upradeMeleeTowerAttackSpeed;
    }

    protected override float GetHealthUpgradeValue()
    {
        return InitValues.upradeMeleeTowerHealth;
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        HudDIncrementPhysicalObjectAmountByOne(PhysicalObjectType.MeleeTower);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        HudDecrementPhysicalObjectAmountByOne(PhysicalObjectType.MeleeTower);
    }
}
