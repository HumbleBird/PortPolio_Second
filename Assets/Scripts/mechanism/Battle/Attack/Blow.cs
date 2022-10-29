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
        Table_Attack.Info info = Managers.Table.m_Attack.Get(id);

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
}
