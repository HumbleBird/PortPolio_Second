using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Player : Character
{
    public void UpdateAnimatorValues(float verticalAmount, float horizontalAmount, bool isSprinting)
    {
        #region Vertical

        float v = 0;
        if(verticalAmount > 0 && verticalAmount < 0.55f)
        {
            v = 0.5f;
        }
        else if (verticalAmount > 0.55f)
        {
            v = 1;
        }
        else if (verticalAmount < 0 && verticalAmount > -0.55f)
        {
            v = -0.5f;
        }
        else if(verticalAmount < -0.55f)
        {
            v = -1;
        } 
        else
        {
            v = 0;
        }

        #endregion

        #region Horizontal

        float h = 0;
        if (horizontalAmount > 0 && horizontalAmount < 0.55f)
        {
            h = 0.5f;
        }
        else if (horizontalAmount > 0.55f)
        {
            h = 1;
        }
        else if (horizontalAmount < 0 && horizontalAmount > -0.55f)
        {
            h = -0.5f;
        }
        else if (horizontalAmount < -0.55f)
        {
            h = -1;
        }
        else
        {
            h = 0;
        }

        #endregion

        if(isSprinting)
        {
            v = 2;
            h = horizontalAmount;
        }

        m_Animator.SetFloat("Vertical", v, 0.1f, Time.deltaTime);
        m_Animator.SetFloat("Horizontal", h, 0.1f, Time.deltaTime);
    }

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
                PlayAnimation($"Dead{deadId}");
                break;
            default:
                break;
        }
    }
}
