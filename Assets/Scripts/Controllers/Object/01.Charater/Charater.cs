using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Charater : Base
{
	public CreatureState State = CreatureState.Idle;
    [HideInInspector] public bool waiting = false;

    protected virtual void Start()
    {
        MaxHp = Hp;
        MaxStamina = Stamina;

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


    public void ResetAnimation(string animname)
    {
        Animator.SetBool(animname, false);
        waiting = false;
    }
}
