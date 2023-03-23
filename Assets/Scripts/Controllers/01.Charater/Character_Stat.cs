using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Character : Base
{
    protected virtual void SetHp(int NewHp, GameObject attacker)
    {
        m_Stat.m_iHp = NewHp;
        if (m_Stat.m_iHp < 0)
        {
            m_Stat.m_iHp = 0;
            OnDead(attacker);
        }
    }

    protected virtual void SetStemina(float NewSetStamina)
    {
        m_Stat.m_fStemina= Mathf.Clamp(NewSetStamina, 0, m_Stat.m_fMaxStemina );
    }

    public virtual void SetMoveState(MoveState state)
    {
        if (eMoveState == state)
            return;

        switch (state)
        {
            case MoveState.None:
                eMoveState = MoveState.None;
                m_Stat.m_fMoveSpeed = 0;
                break;
            case MoveState.Walk:
                eMoveState = MoveState.Walk;
                m_Stat.m_fMoveSpeed = m_Stat.m_fWalkSpeed;
                break;
            case MoveState.Run:
                eMoveState = MoveState.Run;
                m_Stat.m_fMoveSpeed = m_Stat.m_fRunSpeed;
                break;
        }

        UpdateAnimation();
    }

}
