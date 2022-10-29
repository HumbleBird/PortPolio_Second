using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Blow : Attack
{
    // 기본 좌클릭 공격
    public override void BasicAttack(int id = 1)
    {
        info = Managers.Table.m_Attack.Get(id);

        if (info == null)
        {
            Debug.LogError($"해당하는 {id}의 스킬이 없습니다.");
            return;
        }

        m_cGo.Animator.SetBool(info.m_sAnimName, true);

        m_cGo._isNextCanAttack = true;
        m_cGo.m_fCoolTime = info.m_fCoolTime;
    }



    public override void Skill() { }

    public override void CanNextAttack(Action action, int id)
    {
        base.CanNextAttack(action, id);
    }

    public override void StrongAttack(int id = 4)
    {
        throw new NotImplementedException();
    }
}
