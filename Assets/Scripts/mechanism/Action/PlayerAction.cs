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
            { Managers.InputKey._binding.Bindings[UserAction.Jump], Jump},

            { Managers.InputKey._binding.Bindings[UserAction.Crounch], Crounch},
            
            // UI
            { Managers.InputKey._binding.Bindings[UserAction.UI_Setting], ShowInputKeySetting},
        };
    }



    public void Shield()
    {
        m_sActionName = "Shield";
    }

    public void Crounch()
    {
        m_cGo.MoveSpeed = m_cGo.CrounchSpeed;
    }

    public void CrounchShiled()
    {
    }

    public void Jump()
    {
        m_cGo.Animator.SetBool("Action Move", true);
        m_cGo.waiting = true;

        // 제자리 점프
        if (m_cGo.State == CreatureState.Idle)
        {
            m_cGo.Animator.SetFloat("Action Move State", 0);
        }
        // 이동 점프
        else if (m_cGo.State == CreatureState.Move)
        {
            m_cGo.Animator.SetFloat("Action Move State", 1);
        }

        m_cGo.Stop(0.833f); //  애니메이션 길이
        m_cGo.State = CreatureState.Idle;
    }

    #region 미구현
    // 나중에 쓸지는 몰?루
    public void Roll()
    {
        m_cGo.waiting = true;

        if (m_cGo.State == Define.CreatureState.Idle)
        {
            m_cGo.Animator.SetBool("Stand To Roll", true);
        }
        else if (m_cGo.State == Define.CreatureState.Move)
        {
            m_cGo.Animator.SetBool("Run To Roll", true);
        }
    }
    #endregion
}
