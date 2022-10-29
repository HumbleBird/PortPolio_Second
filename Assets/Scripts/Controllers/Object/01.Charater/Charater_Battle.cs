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

        //Debug.Log("다음 연속 공격을 할 것을 정하세요.");
        
        //while (true)
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        Debug.Log("왼쪽 2타");
        //        break;
        //    }
        //    else if (Input.GetMouseButtonDown(1))
        //    {
        //        Debug.Log("오른쪽 강한 1타");
        //        break;
        //    }
        //    //_attack.CanNextAttack(() => _attack.BasicAttack, info.m_iNextNum);
        //    // 플레이어 - 공격 키를 눌렀는가,
        //    // 메커니즘 - 다음 공격 번호가 있는가, 애니메이션이 끝나지 않았는가
        //}

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
