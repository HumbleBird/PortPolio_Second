using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

public class ActionMove : Strategy
{

    public void PlayerActionMove(string action, AnimationBlendState State)
    {
        if (State == AnimationBlendState.Start)
            m_cGo.Animator.SetBool($"{action} Start", true);
        else if (State == AnimationBlendState.Idle)
        {
            switch(action)
            {
                case "Shield":
                    Shield();
                    break;
                case "Crounch":
                    Shield();
                    break;
                case "CrounchShiled":
                    Shield();
                    break;
            }
        }
        else if (State == AnimationBlendState.End)
            m_cGo.Animator.SetBool($"{action} Start", false);

        m_cGo.Animator.SetFloat($"{action} State", (int)State);
    }

    public void Shield()
    {
        
    }

    public void Crounch(bool shileding = false)
    {
        m_cGo.MoveSpeed = m_cGo.CrounchSpeed;
        if(shileding == true)
        {
            // 쉴드
        }
    }

    public void CrounchShiled()
    {
    }

    public void Jump()
    {
        m_cGo.Animator.SetBool("Action Move Start", true);
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

    ////UI
    // Test
    // 디폴트
    // Input 설정창
    bool _ispopupsetting = false;
    public void ShowInputKeySetting()
    {
        UI_SettingKey popup = Managers.UIBattle.UISetting;

        if (_ispopupsetting == false)
        {
            popup.gameObject.SetActive(true);
            _ispopupsetting = true;
        }
        else
        {
            popup.gameObject.SetActive(false);
            _ispopupsetting = false;
        }
    }
}
