using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Character : Base
{
    float m_fNormalizeTransitionTime = 0.1f;
    protected int BaseLayer = 0;
    protected int UpperLayer = 1;

    protected void UpdateAnimation()
    {
        switch (eMoveState)
        {
            case CreatureMoveState.None:
                StandStateAnimation();
                break;
            case CreatureMoveState.Crouch:
                CrouchAnimation();
                break;
        }
    }

    void StandStateAnimation()
    {
        switch (eState)
        {
            case CreatureState.Idle:
                Animator.CrossFade("Idle", m_fNormalizeTransitionTime);
                break;
            case CreatureState.Move:
                if (Sprint == WalkSprint)
                    Animator.CrossFade("Walk", m_fNormalizeTransitionTime);
                else if (Sprint == RunSprint)
                    Animator.CrossFade("Run", m_fNormalizeTransitionTime);
                break;
            case CreatureState.Skill:
                break;
            case CreatureState.Dead:
                Animator.CrossFade("Dead", m_fNormalizeTransitionTime);
                break;
        }
    }

    void CrouchAnimation()
    {
        switch (eState)
        {
            case CreatureState.Idle:
                Animator.CrossFade("Crouch Idle", m_fNormalizeTransitionTime);
                break;
            case CreatureState.Move:
                Animator.CrossFade("Crouch Walk Forward", m_fNormalizeTransitionTime);
                break;
        }
    }
}
