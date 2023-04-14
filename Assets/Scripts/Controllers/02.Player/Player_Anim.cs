using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Player : Character
{
    public override void UpdateAnimation()
    {
        base.UpdateAnimation();

        switch (eState)
        {
            case CreatureState.Idle:
                break;
            case CreatureState.Move:
                break;
            case CreatureState.Skill:
                break;
            case CreatureState.Dead:
                int deadId = UnityEngine.Random.Range(0, 2);
                m_Animator.Play($"Dead{deadId}");
                break;
            default:
                break;
        }
    }

    protected override void MoveAnim()
    {
        if(m_bLockOnFlag)
        {

        }
        else
        {
            switch (eMoveState)
            {
                case MoveState.None:
                    PlayAnimation("Empty");
                    break;
                case MoveState.Walk:
                    PlayAnimation("Walk");
                    break;
                case MoveState.Run:
                    PlayAnimation("Run");
                    break;
                case MoveState.Falling:
                    PlayAnimation("Falling Idle");
                    break;
                default:
                    break;
            }
        }
    }
}
