using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum PlayerActionMoveState
    {
        Start = 0,
        Idle = 1,
        Hit = 2,
        End = 3,
        None = 4
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
        Drag,
    }
}
