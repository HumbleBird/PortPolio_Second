using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Charater : Base
{
    // 접근점
    protected Attack _attack;
    public float m_fCoolTime = 0f;
    public bool _isNextCanAttack = true;

    public GameObject target { get; set; } // 타겟
    public ActionState _actionState = ActionState.None;

    public void ChangeClass(string typeClass)
    {
        switch (typeClass)
        {
            case "Blow":
                _attack = new Blow();
                break;
            case "Range":
                _attack = new Range();
                break;
            default:
                break;
        }
    }

    public virtual void HitEvent()
    {
        
    }

    public virtual void CanNextAttack(int id)
    {
        Table_Attack.Info info = Managers.Table.m_Attack.Get(id);
        Animator.SetBool(info.m_sAnimName, false);

        State = CreatureState.Idle;
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
