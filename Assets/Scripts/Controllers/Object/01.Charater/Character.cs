using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Character : Base
{
	public CreatureState State = CreatureState.Idle;
    [HideInInspector] 
    public bool waiting = false;

    [HideInInspector]
    public GameObject m_GoAttackItem;

    protected virtual void Start()
    {
        // Stat Init
        MaxHp = Hp;
        MaxStamina = Stamina;

        // AttackInfo Init
        _attack.Init(gameObject);
    }

    protected virtual void Update()
    {
        UpdateController();
    }

    protected virtual void UpdateController()
    {
        switch (State)
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
        Animator.Play("Dead");
        Managers.Object.Remove(ID);
        Destroy(gameObject, 5);
    }

    public void CanMove()
    {
        waiting = false;
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
