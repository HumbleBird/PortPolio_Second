﻿using System;
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

    public override void Init() { }
    public abstract void NormalAttack();
    public abstract void AttackEnd();


    // 넉백 효과
    void Kick()
    {
        Vector3 moveDirection = m_cGo.transform.position - m_cTarget.gameObject.transform.position;
        m_cTarget.Rigid.AddForce(moveDirection.normalized * -100f);
    }

    void RefreshTargetSet()
    {
        m_cTarget = m_cGo.m_goTarget;
    }

    public void Shield()
    {
        if (m_cGo.m_strStat.m_fStemina == 0)
            return;

        if (m_cGo.eMoveState == MoveState.Run)
            m_cGo.SetMoveState(MoveState.Walk);

        m_cGo.eActionState = ActionState.Shield;
    }

    // Action
    public void Crouch()
    {
        m_cGo.SetMoveState(MoveState.Crouch);
    }

    public void Roll()
    {
        m_cGo.eActionState = ActionState.Invincible;
    }
}
