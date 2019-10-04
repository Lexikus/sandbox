using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public abstract class Minion : AiObject
{
    [Header("Audio")]
    [SerializeField]
    protected AudioClip runClip;

    [SyncVar]
    protected GameObject finalTarget;
    [SyncVar]
    protected GameObject currentTarget;

    protected NavMeshAgent agent;
    protected Animator anim;
    protected AudioSource runAudio;

    public override void OnStartAuthority()
    {
        InitFirstEnemyTarget();
    }

    protected override void Start()
    {
        base.Start();
        InitAgent();
        InitWalkAudio();
        InitAnimationSettings();
        InitMinionAppearance();
    }

    protected override void Update()
    {
        base.Update();
        MoveMinion();
    }

    private void InitAgent()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null) return;

        agent.speed = MovemenetSpeed;
        agent.stoppingDistance = (MinRange + MaxRange) / 2;
    }

    private void InitWalkAudio()
    {
        if (runClip == null) return;
        runAudio = gameObject.AddComponent<AudioSource>();
        if (runAudio == null) return;

        runAudio.clip = runClip;
        runAudio.priority = 256;
        runAudio.volume = 0.01f;
        runAudio.loop = true;
    }

    private void InitAnimationSettings()
    {
        anim = GetComponent<Animator>();
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

    private void InitFirstEnemyTarget()
    {

        if (enemyBase == null) return;
        SetFinalTarget(enemyBase.gameObject);
    }

    private void MoveMinion()
    {

        if (agent == null) return;
        if (finalTarget == null) return;
        if (currentTarget == null || currentTarget.GetComponent<PhysicalObject>().IsDead())
        {
            SetNewTarget(finalTarget);
        }
        if (currentTarget == null) return;
       
        //Debug.DrawLine(this.transform.position, currentTarget.transform.position, Color.black);

        if (Vector3.Distance(this.transform.position, currentTarget.transform.position) <= MaxRange
         && Vector3.Distance(this.transform.position, currentTarget.transform.position) >= MinRange
            )
        {
            agent.velocity = Vector3.zero;
            StopRun();
        }

        if (Vector3.Distance(this.transform.position, currentTarget.transform.position) > MaxRange)
        {
            agent.destination = currentTarget.transform.position;
            StartRun();
        }

        if (Vector3.Distance(this.transform.position, currentTarget.transform.position) < MinRange)
        {
            agent.destination = (this.transform.position - currentTarget.transform.position).normalized * ((MinRange + MaxRange) / 2) + this.transform.position;
            StartRun();
        }
    }

    public override bool IsDead()
    {
        bool dead = base.IsDead();
        if (dead)
        {
            BeDead();
        }
        return dead;
    }

    private void BeDead()
    {
        AgentFullStop();

        foreach (GameObject arrow in ToolBox.FindChildObjects(this.gameObject, "Arrow(Clone)"))
        {
            arrow.transform.rotation = Quaternion.Euler(new Vector3(90, 0));
            arrow.transform.localPosition = new Vector3(0, 0.1f, 0);
        }
        SetFinalTarget(this.gameObject);
        anim.SetTrigger(isDeadId);
    }

    protected void StartRun()
    {
        if (runAudio == null) return;
        if (anim == null) return;

        if (!runAudio.isPlaying)
        {
            runAudio.Play();
        }
        anim.SetFloat(moveId, MovemenetSpeed);
    }

    protected void StopRun()
    {
        if (runAudio == null) return;
        if (anim == null) return;

        runAudio.Stop();
        anim.SetFloat(moveId, 0);
    }

    protected void AgentFullStop()
    {
        StopRun();

        if (agent == null) return;
        agent.destination = this.transform.position;
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
    }

    protected void AgentContinue()
    {
        StartRun();

        if (currentTarget == null) return;
        if (agent == null) return;
        agent.isStopped = false;
        agent.destination = currentTarget.transform.position;
    }

    public override void ApplyDmg(float dmg, GameObject attacker)
    {
        base.ApplyDmg(dmg, attacker);
        if (attacker == null) return;
        if (currentTarget == finalTarget
            && !IsDead())
        {
            SetNewTarget(attacker);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (IsTagRelevant(other))
        {
            base.OnTriggerEnter(other);

            if (currentTarget == finalTarget)
            {
                SetNewTarget(other.gameObject);
            }
        }
    }

    protected void SetNewTarget(GameObject newTarget)
    {
        if (newTarget == null) return;
        if (!hasAuthority) return;
        currentTarget = newTarget;
        CmdSetCurrentTarget(newTarget);
    }

    [Command]
    private void CmdSetCurrentTarget(GameObject currentTarget)
    {
        try
        {
            if (currentTarget == null) return;
            this.currentTarget = currentTarget;
        }
        catch (Exception e)
        {
            RpcServerLog(e.Message);
        }
    }

    public void SetFinalTarget(GameObject newTarget)
    {
        if (newTarget == null) return;
        if (!hasAuthority) return;
        CmdSetFinalTarget(newTarget);
        finalTarget = newTarget;
        SetNewTarget(finalTarget);
    }

    [Command]
    private void CmdSetFinalTarget(GameObject finalTarget)
    {
        try
        {
            if (finalTarget == null) return;
            this.finalTarget = finalTarget;
        }
        catch (Exception e)
        {
            RpcServerLog(e.Message);
        }
    }
}