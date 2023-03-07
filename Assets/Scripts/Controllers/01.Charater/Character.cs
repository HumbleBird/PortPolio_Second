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

    public List<TrigerDetector> m_GoAttackItem { get; set; } = new List<TrigerDetector>();
    public Table_Attack.Info m_tAttackInfo { get; set; } = new Table_Attack.Info();
    public Stat m_strStat { get; set; } = new Stat();
    protected AudioSource audioSource { get; private set; }

    public virtual int m_TotalAttack { get { return m_strStat.m_iAtk; } }
    public virtual int m_TotalDefence { get { return m_strStat.m_iDef; } }

    [HideInInspector] 
    public bool m_bWaiting = false;

    protected Dictionary<string, AnimationClip> m_DicAniactionclip = new Dictionary<string, AnimationClip>();

    protected override void Init()
    {
        base.Init();

        m_strAttack.SetInfo(this);
        m_strStat.Init();

        AttackColliderInit();

        // Audio
        audioSource = Util.GetOrAddComponent<AudioSource>(gameObject);
        audioSource.spatialBlend = 1;
        audioSource.rolloffMode = AudioRolloffMode.Linear;

        AnimationClip[] clips = Animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            m_DicAniactionclip.Add(clip.name, clip);
        }
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

        m_Collider.isTrigger = true;

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
        eState = Define.CreatureState.Dead;
    }

    public float GetAnimationTime(string animName, float time = 1f)
    {
        if (m_DicAniactionclip.ContainsKey(animName) == false)
        {
            Debug.Log("해당 애니메이션 클립이 없습니다." + animName);
            return 0;
        }

        AnimationClip clip = m_DicAniactionclip[animName];
        return clip.length * time;
    }

    public IEnumerator CoGetAnimationTimeAndWait(string animName, float time = 1f, AnimationLayers layer = AnimationLayers.BaseLayer)
    {
        while (true)
        {
            float getTime = GetAnimationTime(animName, time);

            if (getTime != 0)
            {
                getTime *= 0.7f;
                StartCoroutine(Wait(getTime));
                yield break;
            }

            yield return null;
        }
    }

    protected virtual void SpeicialAction(Action action, bool wating = true)
    {
        // Action 실행
        string animName = action.Method.Name;
        action.Invoke();

        // 애니메이션 실행
        animName = ActionAnimation(animName);

        // 애니메이션 실행전까지 대기
        StartCoroutine(CoGetAnimationTimeAndWait(animName));

    }
}
