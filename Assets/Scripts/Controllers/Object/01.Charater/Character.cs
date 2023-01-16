using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Character : Base
{
    public ActionState eActionState = ActionState.None;
    public MoveState eMoveState = MoveState.None;
    private CreatureState state = CreatureState.Idle;
    public virtual CreatureState eState
    {
        get { return state; }
        set
        {
            if (state == value)
                return;

            state = value;
            UpdateAnimation();
        }
    }

    public List<TrigerDetector> m_GoAttackItem { get; set; } = new List<TrigerDetector>();
    public Table_Attack.Info m_tAttackInfo { get; set; } = new Table_Attack.Info();
    public Stat m_strStat { get; set; } = new Stat();

    [HideInInspector] 
    public bool m_bWaiting = false;

    protected override void Init()
    {
        base.Init();

        m_strAttack.SetInfo(gameObject);
        m_strStat.Init();
    }

    void Update()
    {
        UpdateController();
    }

    protected virtual void UpdateController()
    {
        switch (eState)
        {
            case CreatureState.Idle:
                UpdateIdle();
                break;
            case CreatureState.Move:
                UpdateMove();
                break;
            case CreatureState.Skill:
                UpdateSkill();
                break;
            case CreatureState.Dead:
                UpdateDead();
                break;
        }
    }

    protected virtual void UpdateIdle() { }
    protected virtual void UpdateMove() { }

    protected virtual void UpdateSkill() 
    {
		Coroutine co = StartCoroutine(AttackEndCheck());
    }

    protected virtual void UpdateDead() 
    {
        Rigid.isKinematic = true;
        Managers.Object.Remove(ID);
        Destroy(gameObject, 5);

        // TODO Regdoll
    }

    public void Stop(float duration)
    {
        if (m_bWaiting)
            return;

        StartCoroutine(Wait(duration));
    }

    IEnumerator Wait(float duration)
    {
        m_bWaiting = true;
        yield return new WaitForSecondsRealtime(duration);
        m_bWaiting = false;
    }
}
