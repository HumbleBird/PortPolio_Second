using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

public abstract class Attack : Strategy
{
    public Table_Attack.Info info;
    int KickId = 51;

    public override void SetKeyMehod()
    {
        base.SetKeyMehod();

        keyDictionary = new Dictionary<KeyCode, Action>
        {
            { Managers.InputKey._binding.Bindings[UserAction.Kick], Kick},
        };
    }

    public virtual void BasicAttack(int id = 1) 
    {
        CheckCooltime();
    }

    public virtual void StrongAttack(int id = 4)
    {
    }

    public abstract void Skill();
    public virtual void Kick() 
    {
        info = Managers.Table.m_Attack.Get(KickId);

        m_cGo.Animator.SetTrigger("Kick");
        m_cGo._isNextCanAttack = true;
        m_cGo.m_fCoolTime = info.m_fCoolTime;
        m_cGo.Atk += info.m_fDmg;

        // 공격 판정(범위)
        // 공격력 설정
        // 보스 제외 넉백
    }

    void CheckCooltime()
    {
        if (m_cGo.m_fCoolTime > 0)
            return;
    }
}
