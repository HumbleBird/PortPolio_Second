using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

public class Attack : Strategy
{
    public Table_Attack.Info info;
    int m_iAttackId;
    int kickId = 51;

    public void AttackInfoCal(int id)
    {
        CheckCooltime();

        info = Managers.Table.m_Attack.Get(id);

        if (info == null)
        {
            Debug.LogError($"해당하는 {id}의 스킬이 없습니다.");
            return;
        }

        m_cGo._isNextCanAttack = false;
        m_cGo.eState = CreatureState.Skill;

        m_cGo.Animator.SetBool(info.m_sAnimName, true);

        m_iAttackId = id;

        m_cGo._isNextCanAttack = true;
        m_cGo.m_fCoolTime = info.m_fCoolTime;
        m_cGo.Atk += info.m_fDmg;
    }

    public void SpecialAddAttackInfo()
    {
        RefreshTargetSet();

        if (m_iAttackId == kickId)
            Kick();

        m_iAttackId = -1;
    }

    void CheckCooltime()
    {
        if (m_cGo.m_fCoolTime > 0)
            return;
    }

    // 넉백 효과
    void Kick()
    {
        Vector3 moveDirection = m_cGo.transform.position - m_GoTarget.transform.position;
        m_cTarget.Rigid.AddForce(moveDirection.normalized * -100f);
    }

    void RefreshTargetSet()
    {
        m_GoTarget = m_cGo.m_goTarget;
        m_cTarget = m_GoTarget.GetComponent<Base>();
    }
}
