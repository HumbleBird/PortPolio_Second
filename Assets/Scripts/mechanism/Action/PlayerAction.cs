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
            
        };

        OnekeyDictionary = new Dictionary<KeyCode, Action>
        {
            // 액션
            //Managers.InputKey._binding.Bindings[UserAction.Jump], Jump},
            { Managers.InputKey._binding.Bindings[UserAction.Roll], Roll},
        
            // UI
            { Managers.InputKey._binding.Bindings[UserAction.UI_Setting], ShowInputKeySetting},
            { Managers.InputKey._binding.Bindings[UserAction.UI_Inventory], ShowInventory},
        };
    }


    public void Shield()
    {
        if (m_cGo.Stamina == 0)
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
