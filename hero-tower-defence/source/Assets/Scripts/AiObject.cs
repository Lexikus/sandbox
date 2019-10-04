using UnityEngine;

public class AiObject : PhysicalObject
{
    protected override void Start()
    {
        base.Start();
        this.GetComponent<SphereCollider>().radius = MaxRange;
    }

    public bool IsTagRelevant(Collider other)
    {
        if (tag == "Untagged") return false;
        if (other.tag == this.tag) return false;
        if (other.tag == "Untagged") return false;
        if (other.isTrigger) return false;
        if (other.gameObject.GetComponent<PhysicalObject>() == null) return false;
        return true;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (IsTagRelevant(other))
        {
            CmdAddEnemy(other.gameObject);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (IsTagRelevant(other))
        {
            CmdRemoveEnemy(other.gameObject);
        }
    }

    protected void HudDIncrementPhysicalObjectAmountByOne(PhysicalObjectType type)
    {
        if (!hasAuthority) return;
        if (HUD.Instance == null) return;
        HUD.Instance.IncrementPhysicalObjectAmountByOne(type);
    }

    protected void HudDecrementPhysicalObjectAmountByOne(PhysicalObjectType type)
    {
        if (!hasAuthority) return;
        if (HUD.Instance == null) return;
        HUD.Instance.DecrementPhysicalObjectAmountByOne(type);
    }
}
