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

        m_cGo.SetStaminaGraduallyFillingUp(false);
        m_cGo.StrAnimation(info.m_sAnimName);

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

        int id = info.m_nID;

        if (id == m_cGo.m_iKickNum)
            Kick();
        else if (id == m_cGo.m_iBasicAttackNum)
            BasicAttack();
        else if (id == m_cGo.m_iStrongAttackNum)
            StrongAttack();
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

    public void AttackAtkReset()
    {
        m_cGo.Atk -= info.m_fDmg;
    }
}
