using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Hero : PhysicalObject
{
    private Animator anim;
    private NavMeshAgent agent;
    private GameObject targetEnemy;
    private PhysicalObject targetReference;
    private RaycastHit hit;
    private const float stand = 0;
    private bool enemyInRange;

    [Header("Audio")]
    [SerializeField] private AudioClip swordSlashClip;
    protected AudioSource audioSource;

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        InitMinionAppearance();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void Update()
    {
        if (!hasAuthority) return;

        if (Input.GetMouseButtonDown(0) && !this.IsDead())
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                HeroMove(hit);
                GetTarget(hit);
            }
        }

        if (enemyInRange && targetReference != null && !targetReference.IsDead())
        {
            Attack();
        }

        if (Vector3.Distance(agent.transform.position, agent.destination) <= 0.1f)
        {
            anim.SetFloat(moveId, stand);
        }
    }

    public override bool IsDead()
    {
        bool dead = base.IsDead();
        if (dead)
        {
            anim.SetTrigger("IsDead");
            agent.destination = transform.position;
        }
        return dead;
    }

    private void HeroMove(RaycastHit hit)
    {
        agent.destination = hit.point;
        anim.SetFloat(moveId, MovemenetSpeed);
        agent.speed = MovemenetSpeed;
    }

    private void GetTarget(RaycastHit hit)
    {
        if (!hit.transform.CompareTag(this.gameObject.tag))
        {
            targetEnemy = hit.transform.gameObject;
            targetReference = targetEnemy.GetComponent<PhysicalObject>();
        }
        else
        {
            targetEnemy = null;
            targetReference = null;
        }
    }

    private void Attack()
    {
        InitValues.PlayLowPrioAudioSource(this.gameObject, swordSlashClip, 0, 0);
        anim.SetTrigger(isAttackingId);
        anim.SetFloat(attackingSpeedId, AttackSpeed);
    }

    public override void ApplyDmgFromAnimation()
    {
        targetReference.ApplyDmg(this.AttackPower, this.gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == targetEnemy)
        {
            enemyInRange = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        enemyInRange = false;
    }

    private void InitMinionAppearance()
    {
        switch (tag)
        {
            case "Alpha":
                transform.Find("Base").gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                break;
            case "Omega":
                transform.Find("Base").gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                break;
            default:
                Debug.Log("Wrong Tag: " + tag);
                break;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (!hasAuthority) return;
        myBase.HeroRespawn();
    }
}