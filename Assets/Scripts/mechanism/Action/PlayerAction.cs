using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

public partial class PlayerAction : Strategy
{

    public override void SetKeyMehod()
    {
        keyDictionary = new Dictionary<KeyCode, Action>
        {
            // 액션
            //{ Managers.InputKey._binding.Bindings[UserAction.Jump], Jump},

            { Managers.InputKey._binding.Bindings[UserAction.Crouch], Crouch},
            { Managers.InputKey._binding.Bindings[UserAction.Shield], Shield},
            
            // UI
            { Managers.InputKey._binding.Bindings[UserAction.UI_Setting], ShowInputKeySetting},
        };
    }

    public void Shield()
    {
        if(m_cGo.Stamina >= 0 && m_cGo.Animator.GetBool("Crouch") == false)
        {
            m_sActionName = "Shield";
        }
    }

    public void Crouch()
    {
        m_sActionName = "Crouch";
        m_cGo.MoveSpeed = m_cGo.CrounchSpeed;
    }

    public void Jump()
    {
        m_sActionName = "Action Move";
        m_cGo.waiting = true;

        // 제자리 점프
        if (m_cGo.eState == CreatureState.Idle)
        {
            m_cGo.Animator.SetFloat("Action Move eState", 0);
        }
        // 이동 점프
        else if (m_cGo.eState == CreatureState.Move)
        {
            m_cGo.Animator.SetFloat("Action Move eState", 1);
        }

        m_cGo.Stop(0.833f); //  애니메이션 길이
        m_cGo.eState = CreatureState.Idle;
    }

    #region 미구현
    // 나중에 쓸지는 몰?루
    public void Roll()
    {
        m_cGo.waiting = true;

        if (m_cGo.eState == Define.CreatureState.Idle)
        {
            m_cGo.Animator.SetBool("Stand To Roll", true);
        }
        else if (m_cGo.eState == Define.CreatureState.Move)
        {
            m_cGo.Animator.SetBool("Run To Roll", true);
        }
    }
    #endregion
}
