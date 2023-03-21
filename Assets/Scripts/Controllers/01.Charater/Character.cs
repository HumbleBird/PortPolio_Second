using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public partial class Character : Base
{
    #region Character State Enum
    public ActionState eActionState = ActionState.None;
    public MoveState eMoveState = MoveState.None;
    private CreatureState state = CreatureState.Idle;
    protected CharacterClass eCharacterClass = CharacterClass.None;
    public virtual CreatureState eState
    {
        get { return state; }
        set
        {
            if (state == value)
                return;

            state = value;
            UpdateAnimation();
            UpdateSound();
        }
    }
    #endregion

    #region Variable
    public List<TrigerDetector> m_GoAttackItem { get; set; } = new List<TrigerDetector>();
    public Table_Attack.Info m_tAttackInfo { get; set; } = new Table_Attack.Info();
    public Stat m_strStat { get; set; } = new Stat();
    protected AudioSource audioSource { get; private set; }

    public virtual int m_TotalAttack { get { return m_strStat.m_iAtk; } }
    public virtual int m_TotalDefence { get { return m_strStat.m_iDef; } }

    [HideInInspector] 
    public bool m_bWaiting = false;

    protected Dictionary<string, AnimationClip> m_DicAniactionclip = new Dictionary<string, AnimationClip>();
    #endregion

    protected override void Init()
    {
        base.Init();

        SetAudio();
        SetAnimation();
        SetAttackInfo();
        SetInfo();
        m_strStat.Init();
        m_cAttack.SetInfo(this);
    }

    void Update()
    {
        UpdateController();
    }

    #region SetInfo
    protected virtual void SetInfo()
    {
        Managers.Object.Add(ID, gameObject);
    }

    void SetAnimation()
    {
        AnimationClip[] clips = m_Animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (!m_DicAniactionclip.ContainsKey(clip.name))
                m_DicAniactionclip.Add(clip.name, clip);
        }
    }

    private void SetAudio()
    {
        // Audio
        audioSource = Util.GetOrAddComponent<AudioSource>(gameObject);
        audioSource.spatialBlend = 1;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
    }

    void SetAttackInfo()
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

    #endregion

    #region Creature State Controller

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
        m_Rigid.isKinematic = true;
        Managers.Object.Remove(ID);

        m_Collider.isTrigger = true;

    }

    #endregion

    #region Order

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

    public virtual void OnDead(GameObject Attacker)
    {
        eState = Define.CreatureState.Dead;
    }



    #endregion
}
