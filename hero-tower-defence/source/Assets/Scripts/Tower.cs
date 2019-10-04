public abstract class Tower : AiObject
{
    public int ID { get; private set; }

    protected override void Start()
    {
        base.Start();
        ID = unchecked((int)this.netId.Value); //unchecked just to be sure no OverflowException appears
        RegisterEvents();
    }

    protected void UpdateTowerHUD()
    {
        if (HUD.Instance == null) return;
        HUD.Instance.UpdateInfoTowerBox(this);
    }

    private void RegisterEvents()
    {
        if (HUD.Instance == null) return;
        HUD.Instance.OnUpradeTowerAttackPower += UpradeTowerAttackPower;
        HUD.Instance.OnUpradeTowerAttackSpeed += UpradeTowerAttackSpeed;
        HUD.Instance.OnUpradeTowerHealth += UpradeTowerHealth;
    }

    protected abstract float GetAttackPowerUpgradeValue();
    protected abstract float GetAttackSpeedUpgradeValue();
    protected abstract float GetHealthUpgradeValue();

    private void UpradeTowerAttackPower(int id)
    {
        if (myBase == null) return;
        if (id == this.ID)
        {
            if (!myBase.TryToPay(UpgradeGoldCosts)) return;
            AttackPower += GetAttackPowerUpgradeValue();
            UpdateTowerHUD();
        }
    }

    protected void UpradeTowerAttackSpeed(int id)
    {
        if (myBase == null) return;
        if (id == this.ID)
        {
            if (!myBase.TryToPay(UpgradeGoldCosts)) return;
            AttackSpeed += GetAttackSpeedUpgradeValue();
            UpdateTowerHUD();
        }
    }

    protected void UpradeTowerHealth(int id)
    {
        if (myBase == null) return;
        if (id == this.ID)
        {
            if (!myBase.TryToPay(UpgradeGoldCosts)) return;
            MaxHealth += GetHealthUpgradeValue();
            UpdateTowerHUD();
        }
    }
}
