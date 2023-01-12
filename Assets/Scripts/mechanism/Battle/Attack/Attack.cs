using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

public class Attack : Strategy
{
    public Table_Attack.Info info;

    public int m_iBasicAttackNum = 1;
    public int m_iStrongAttackNum = 4;
    //public int m_iCrouchAttackNum = 7;
    public int m_iKickNum = 501;

    protected virtual void BasicAttack() { }
    protected virtual void StrongAttack() { }

    // 넉백 효과
    void Kick()
    {
        Vector3 moveDirection = m_cGo.transform.position - m_GoTarget.transform.position;
        m_cTarget.Rigid.AddForce(moveDirection.normalized * -100f);
    }

    public void SpecialAddAttackInfo()
    {
        RefreshTargetSet();

        int id = info.m_nID;

        if (id == m_iKickNum)
            Kick();
        else if (id == m_iBasicAttackNum)
            BasicAttack();
        else if (id == m_iStrongAttackNum)
            StrongAttack();
    }

    void RefreshTargetSet()
    {
        m_GoTarget = m_cGo.m_goTarget;
        m_cTarget = m_GoTarget.GetComponent<Base>();
    }

    //Action
    public virtual void SetKeyMehod()
    {
        MaintainkeyDictionary = new Dictionary<KeyCode, Action>
        {
            { Managers.InputKey._binding.Bindings[UserAction.Crouch], Crouch},
            { Managers.InputKey._binding.Bindings[UserAction.Shield], Shield},

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
        if (m_cGo.m_strStat.m_fStemina == 0)
            return;

        if (m_cGo.eMoveState == MoveState.Crouch)
            m_sAnimationName = "Crouch Shield";
        else if (m_cGo.eMoveState == MoveState.Run)
        {
            m_cGo.SetMoveState(MoveState.Walk);
            m_sAnimationName = "Shield";
        }
        else
            m_sAnimationName = "Shield";

        m_eAnimLayers = Layers.UpperLayer;
    }

    public void Crouch()
    {
        m_cGo.m_bWaiting = true;
        m_sAnimationName = "Crouch";
        m_cGo.SetMoveState(MoveState.Crouch);
    }

    public void Jump()
    {

    }

    public void Roll()
    {
        m_cGo.eActionState = ActionState.Invincible;

        if (m_cGo.eState == CreatureState.Idle)
            m_sAnimationName = "Stand To Roll";
        else if (m_cGo.eState == CreatureState.Move)
            m_sAnimationName = "Run To Roll";
    }
}
