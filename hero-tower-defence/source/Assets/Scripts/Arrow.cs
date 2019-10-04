using UnityEngine;
using UnityEngine.Networking;

public class Arrow : NetworkBehaviour
{
    [SerializeField]
    protected AudioClip arrowHitClip;

    private GameObject source;
    private float dmg;

    private void Start()
    {
        Destroy(this.gameObject, 10f);
    }

    private void Update()
    {
        if (!hasAuthority) return;
        this.transform.Translate((Vector3.forward + new Vector3(0, -0.05f)) * Time.deltaTime * 20);
    }

    public void SetDestination(GameObject target, GameObject source, float dmg)
    {
        if (target == null) return;
        this.source = source;
        this.dmg = dmg;
        this.transform.LookAt(target.transform.position + new Vector3(0, 1, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasAuthority) return;
        if (other.isTrigger) return;
        if (other.gameObject.name == this.gameObject.name) return;
        if (other.tag == this.tag) return;
        if (other == null) return; 

        PhysicalObject physicalObject = other.gameObject.GetComponent<PhysicalObject>();
        if (physicalObject != null)
        {
            if (physicalObject.IsDead()) return;
            physicalObject.ApplyDmg(dmg, source);
        }

        this.transform.Translate(Vector3.forward * 0.75f);
        this.transform.parent = other.gameObject.transform;

        InitValues.PlayLowPrioAudioSource(gameObject, arrowHitClip, 0, 0.4f);

        this.enabled = false;
    }
}
