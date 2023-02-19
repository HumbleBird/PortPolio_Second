using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Character : Base
{
    protected virtual void SetHp(int NewHp, GameObject attacker)
    {
        m_strStat.m_iHp = NewHp;
        if (m_strStat.m_iHp < 0)
        {
            m_strStat.m_iHp = 0;
            OnDead(attacker);
        }
    }

    protected virtual void SetStemina(float NewSetStamina)
    {
        m_strStat.m_fStemina= Mathf.Clamp(NewSetStamina, 0, m_strStat.m_fMaxStemina );
    }

    protected IEnumerator UpdateCoolTime()
    {
        while (true)
        {
            m_fCoolTime -= Time.deltaTime;
            if (m_fCoolTime < 0)
                m_fCoolTime = 0;

            yield return null;
        }
    }

    public virtual void SetMoveState(MoveState state)
    {
        if (eMoveState == state)
            return;

        switch (state)
        {
            case MoveState.None:
                eMoveState = MoveState.None;
                m_strStat.m_fMoveSpeed = 0;
                break;
            case MoveState.Walk:
                eMoveState = MoveState.Walk;
                m_strStat.m_fMoveSpeed = m_strStat.m_fWalkSpeed;
                break;
            case MoveState.Run:
                eMoveState = MoveState.Run;
                m_strStat.m_fMoveSpeed = m_strStat.m_fRunSpeed;
                break;
            case MoveState.Crouch:
                eMoveState = MoveState.Crouch;
                m_strStat.m_fMoveSpeed = m_strStat.m_fCrouchSpeed;
                break;
            default:
                break;
        }

        //navMeshAgent.speed = m_strStat.m_fMoveSpeed;
        UpdateAnimation();
    }

}
