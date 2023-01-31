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
    public CharacterJob eCharacterJob = CharacterJob.None;

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

    public virtual float m_TotalAttack { get { return m_strStat.m_fAtk; } }
    public virtual float m_TotalDefence { get { return m_strStat.m_fDef; } }

    [HideInInspector] 
    public bool m_bWaiting = false;

    protected override void Init()
    {
        base.Init();

        m_strAttack.SetInfo(gameObject);
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
        //m_strStat.m_fHp = m_strStat.m_fMaxHp;
        //m_strStat.m_fMp = m_strStat.m_fMaxMp;
        //state = CreatureState.Idle;
    }
}
