using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public partial class Character : Base
{
    public ActionState eActionState = ActionState.None;
    public MoveState eMoveState = MoveState.None;
    private CreatureState state = CreatureState.Idle;
    public CharacterClass eCharacterClass = CharacterClass.None;
	public NavMeshAgent navMeshAgent;

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

    public virtual int m_TotalAttack { get { return m_strStat.m_iAtk; } }
    public virtual int m_TotalDefence { get { return m_strStat.m_iDef; } }

    [HideInInspector] 
    public bool m_bWaiting = false;

    protected override void Init()
    {
        base.Init();

        navMeshAgent = Util.GetOrAddComponent<NavMeshAgent>(gameObject);

        m_strAttack.SetInfo(this);
        m_strStat.Init();

        AttackColliderInit();

        StartCoroutine(UpdateCoolTime());
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
    protected virtual void UpdateSkill() { }

    protected virtual void UpdateDead() 
    {
        Rigid.isKinematic = true;
        Managers.Object.Remove(ID);

        // TODO
        m_Collider.isTrigger = true;
        // 애니메이션을 체크해서, 애니메이션이 끝나면 시체가 남게
        AnimationFinishAndState(m_sCurrentAnimationName, CreatureState.Dead);
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

    void AttackColliderInit()
    {
        GameObject weapon = Util.FindChild(gameObject, "WeaponAttackCollider", true);
        if (weapon != null)
        {
            TrigerDetector weaponTD = Util.GetOrAddComponent<TrigerDetector>(weapon);
            weaponTD.eAttackCollider = AttackCollider.Weapon;
            weaponTD.Init();

            weapon.GetComponent<Collider>().isTrigger = true;
        }

        GameObject front = Util.FindChild(gameObject, "FrontAttackCollider");
        if (front != null)
        {
            TrigerDetector frontTD = Util.GetOrAddComponent<TrigerDetector>(front);
            frontTD.eAttackCollider = AttackCollider.CharacterFront;
            frontTD.Init();

            front.GetComponent<Collider>().isTrigger = true;
        }
    }

    public virtual void OnDead(GameObject Attacker)
    {
        // TODO For Pooling
        //m_strStat.m_iHp = m_strStat.m_iMaxHp;
        //m_strStat.m_iMp = m_strStat.m_iMaxMp;
        //state = CreatureState.Idle;
    }

    public void AnimationFinishAndState(string animName, CreatureState state = CreatureState.Idle)
    {
        StartCoroutine(CoAnimationFinishAndState(animName, state));
    }

    public IEnumerator CoAnimationFinishAndState(string animName, CreatureState state = CreatureState.Idle)
    {
        if (Animator.GetCurrentAnimatorStateInfo(0).IsName(animName))
        {
            float animationlegth = Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            Stop(animationlegth);
            yield return new WaitForSeconds(animationlegth);
            eState = state;
        }
    }
}
