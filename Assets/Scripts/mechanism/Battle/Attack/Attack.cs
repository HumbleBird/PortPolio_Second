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

    public override void InputOnekey()
    {
        if (Input.anyKeyDown)
        {
            foreach (var dic in OnekeyDictionary)
            {
                if (Input.GetKeyDown(dic.Key))
                {
                    dic.Value();
                    AttackInfoCal(m_iAttackId);
                }
            }
        }
    }

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

        m_cGo.Animator.CrossFade(info.m_sAnimName, m_cGo.m_fNormalizeTransitionDuration);

        m_iAttackId = id;

        m_cGo._isNextCanAttack = true;
        m_cGo.m_fCoolTime = info.m_fCoolTime;
        m_cGo.Atk += info.m_fDmg;
    }

    protected virtual void BasicAttack() { }
    protected virtual void StrongAttack() { }

    // 넉백 효과
    void Kick()
    {
        Vector3 moveDirection = m_cGo.transform.position - m_GoTarget.transform.position;
        m_cTarget.Rigid.AddForce(moveDirection.normalized * -100f);
    }

    public void SpecialAddAttackInfo()
    {
        RefreshTargetSet();

        if (m_iAttackId == m_cGo.m_iKickNum)
            Kick();
        //else if (m_iAttackId == m_cGo.m_iBasicAttackNum || m_iAttackId == m_cGo.m_iStrongAttackNum)
            //IsNextAttack();

        m_iAttackId = -1;
    }

    void IsNextAttack()
    {
        AttackInfoCal(info.m_iNextNum);
    }

    void CheckCooltime()
    {
        if (m_cGo.m_fCoolTime > 0)
            return;
    }

    void RefreshTargetSet()
    {
        m_GoTarget = m_cGo.m_goTarget;
        m_cTarget = m_GoTarget.GetComponent<Base>();
    }

    void AttackAtkReset()
    {
        m_cGo.Atk -= info.m_fDmg;
    }
}
