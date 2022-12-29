using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Character : Base
{
    CreatureMoveState moveState = CreatureMoveState.None;
    public virtual CreatureMoveState eMoveState
    {
        get { return moveState; }
        set
        {
            if (moveState == value)
                return;

            moveState = value;
            UpdateAnimation();
        }
    }

    CreatureState _state = CreatureState.Idle;
    public virtual CreatureState eState
    {
        get { return _state; }
        set
        {
            if (_state == value)
                return;

            _state = value;
            UpdateAnimation();
        }
    }

    protected float WalkSprint = 0.5f;
    protected float RunSprint = 1f;

    float sprint;
    public float Sprint
    {
        get { return sprint; }
        set
        {
            if (sprint == value)
                return;

            sprint = value;

            UpdateAnimation();
        }
    }

    [HideInInspector] 
    public bool waiting = false;

    [HideInInspector]
    public List<TrigerDetector> m_GoAttackItem;

    protected virtual void Start()
    {
        // Stat Init
        MaxHp = Hp;
        MaxStamina = Stamina;
        OriginalAtk = Atk;  

        // AttackInfo Init
        m_strAttack.Init(gameObject);
    }

    protected virtual void Update()
    {
        UpdateController();
        m_fCoolTime -= Time.deltaTime;
        if (m_fCoolTime < 0)
            m_fCoolTime = 0;
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

    protected virtual void UpdateIdle() 
    { 
        Animator.SetFloat("Sprint", 0);
    }
    protected virtual void UpdateMove() { }
    protected virtual void UpdateSkill() { }
    protected virtual void UpdateDead() 
    {
        Rigid.isKinematic = true;
        Animator.Play("Dead");
        Managers.Object.Remove(ID);
        Destroy(gameObject, 5);
    }

    public void Stop(float duration)
    {
        if (waiting)
            return;
        StartCoroutine(Wait(duration));
    }

    IEnumerator Wait(float duration)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(duration);
        waiting = false;
    }
}
