using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

public class Action : Strategy
{
    // 구르기
    // 상호작용 등

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

    public void Crounch(PlayerActionMoveState State)
    {
        if(State == PlayerActionMoveState.Start)
        {
            m_cGo.Animator.SetBool("Crounch Start", true);
            m_cGo.MoveSpeed = m_cGo.CrounchSpeed;
        }
        else if (State == PlayerActionMoveState.End)
        {
            m_cGo.Animator.SetBool("Crounch Start", false);
        }

        m_cGo.Animator.SetFloat("Crounch State", (int)State);
    }

    public void CrounchBlock(PlayerActionMoveState State)
    {
        if (State == PlayerActionMoveState.Start)
            m_cGo.Animator.SetBool("Crounch Block Start", true);
        // 스테미너 회복 감소
        else if (State == PlayerActionMoveState.End)
            m_cGo.Animator.SetBool("Crounch Block Start", false);

        m_cGo.Animator.SetFloat("Crounch Block State", (int)State);
    }

    public void Shiled(PlayerActionMoveState State)
    {
        if (State == PlayerActionMoveState.Start)
            m_cGo.Animator.SetBool("Shield Start", true);
        // 스테미너 회복 감소
        else if (State == PlayerActionMoveState.End)
            m_cGo.Animator.SetBool("Shield Start", false);

        m_cGo.Animator.SetFloat("Shield State", (int)State);
    }
}
