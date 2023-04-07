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
    public Stat m_Stat { get; set; } = new Stat();
    protected AudioSource m_AudioSource { get; private set; }

    protected bool m_bWaiting = false;

    protected Dictionary<string, AnimationClip> m_DicAniactionclip = new Dictionary<string, AnimationClip>();

    public Transform m_LockOnTransform;
    #endregion

    protected override void Init()
    {
        base.Init();

        SetAudio();
        SetAnimation();
        SetInfo();
    }

    protected virtual void Update()
    {
        UpdateController();
    }

    #region SetInfo
    protected virtual void SetInfo()
    {
        Managers.Object.Add(ID, gameObject);
        m_Stat.m_tStatInfo = Managers.Table.m_Stat.Get(ID);
        m_Stat.Init();
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
        m_AudioSource = Util.GetOrAddComponent<AudioSource>(gameObject);
        m_AudioSource.spatialBlend = 1;
        m_AudioSource.rolloffMode = AudioRolloffMode.Linear;
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
        // 죽으면 연기처럼 사라짐.
        // 시체 아이템 드롭은 그냥 줌.
        
        m_Rigidbody.isKinematic = true;
        Managers.Object.Remove(ID);

        m_Collider.enabled = false; 

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

    public virtual void OnDead()
    {
        eState = Define.CreatureState.Dead;
    }

    public virtual void OnDead(GameObject attacker)
    {
        eState = Define.CreatureState.Dead;
    }

    protected IEnumerator WaitToState(float time, CreatureState state)
    {
        yield return new WaitForSeconds(time);

        eState = state;

        yield break;
    }

    #endregion
}
