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

    public int m_iBasicAttackNum = 1;
    public int m_iStrongAttackNum = 4;
    //public int m_iCrouchAttackNum = 7;
    public int m_iKickNum = 501;

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

        int id = info.m_nID;

        if (id == m_iKickNum)
            Kick();
        else if (id == m_iBasicAttackNum)
            BasicAttack();
        else if (id == m_iStrongAttackNum)
            StrongAttack();
    }

    public void CheckCooltime()
    {
        if (m_cGo.m_fCoolTime > 0)
            return;
    }

    void RefreshTargetSet()
    {
        m_GoTarget = m_cGo.m_goTarget;
        m_cTarget = m_GoTarget.GetComponent<Base>();
    }

    public void AttackAtkReset()
    {
        m_cGo.Atk -= info.m_fDmg;
    }
}
