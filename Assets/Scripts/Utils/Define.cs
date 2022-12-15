using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum ActionState
    {
        None,
        Shield,
        Charging,
        Reload,
    }

    public enum UserAction
    {
        //Battle
        BasicAttack,
        StrongAttack,
        Kick,
        Shield,

        //Action
        Jump,
        Crouch,

        // UI
        UI_Inventory,
        UI_Status,
        UI_Skill,
        UI_Setting
    }

    public enum CharaterType
    {
        Player,
        Monster,
        Boss
    }

    public enum Team
    {
        All,
        Player1,
        Player2
    }

    public enum WorldObject
    {
        Unknown,
        Player,
        Monster,
    }

    public enum Layer
    {
        Monster = 8,
        Ground = 9,
        Block = 10,
    }

    public enum CreatureState
    {
        Idle,
        Move,
        Skill,
        Dead
    }

    public enum CameraMode
    {
        QuarterView,
    }

    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Pressed,
        PointerDown,
        PointerUp,
    }
}
