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
        MaintainkeyDictionary = new Dictionary<KeyCode, Action>
        {
            { Managers.InputKey._binding.Bindings[UserAction.Crouch], Crouch},
            { Managers.InputKey._binding.Bindings[UserAction.Shield], Shield},
            
            // UI
            { Managers.InputKey._binding.Bindings[UserAction.UI_Setting], ShowInputKeySetting},
        };

        OnekeyDictionary = new Dictionary<KeyCode, Action>
        {
            // 액션
            //Managers.InputKey._binding.Bindings[UserAction.Jump], Jump},
            { Managers.InputKey._binding.Bindings[UserAction.Roll], Roll},
        };
    }

    public void Shield()
    {
        if(m_cGo.Stamina >= 0)
        {
            m_sActionName = "Shield";
        }
    }

    public void Crouch()
    {
        m_sActionName = "Crouch";
        m_cGo.MoveSpeed = m_cGo.CrounchSpeed;
        m_cGo.eMoveState = CreatureMoveState.Crouch;
    }

    public void Jump()
    {
        m_sActionName = "Action Move";
        m_cGo.waiting = true;

        // 제자리 점프
        if (m_cGo.eState == CreatureState.Idle)
        {
            m_cGo.Animator.SetFloat("Action Move State", 0);
        }
        // 이동 점프
        else if (m_cGo.eState == CreatureState.Move)
        {
            m_cGo.Animator.SetFloat("Action Move State", 1);
        }

        m_cGo.Stop(0.833f); //  애니메이션 길이
        m_cGo.eState = CreatureState.Idle;
    }

    public void Roll()
    {
        m_sActionName = "Action Move";
        m_cGo.waiting = true;

        // 제자리 점프
        if (m_cGo.eState == CreatureState.Idle)
        {
            m_cGo.Animator.SetFloat("Action Move State", 2);
        }
        // 이동 점프
        else if (m_cGo.eState == CreatureState.Move)
        {
            m_cGo.Animator.SetFloat("Action Move State", 3);
        }

        //m_cGo.Stop(0.6f); //  애니메이션 길이
        m_cGo.eState = CreatureState.Idle;
    }
}
