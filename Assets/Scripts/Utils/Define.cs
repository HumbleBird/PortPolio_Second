using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    #region Creature
    public enum ObjectType
    {
        None,
        Player,
        Monster,
        Boss,
        Projectile,
    }

    public enum CharacterClass
    {
        None = 0,
        Warior = 1,
        Knight = 2,
        Archer = 3,
        Wizard = 4
    }

    public enum CreatureState
    {
        Idle,
        Move,
        Skill,
        Dead
    }

    public enum MonsterType
    {
        Skeleton,
        Jombi,
        Mummy, // 미라
        
    }
    #endregion

    #region Multy

    public enum Team
    {
        All,
        Player1,
        Player2
    }
    #endregion

    #region Item
    public enum ConsumableType
    {
        None,
        Consumable
    }

    public enum ArmorType
    {
        None = 0,
        Helmet = 1,
        Armor = 2,
        Pant = 3,
        Gloves = 4,
    }

    public enum WeaponType
    {
        Daggers = 1,
        StraightSwordsGreatswords = 2,
        Greatswords,
        UltraGreatswords,
        CurvedSword,
        Katanas,
        CurvedGreatswords,
        PiercingSwords,
        Axes,
        Greataxes,

        Hammers,
        GreatHammers,
        FistAndClaws,
        SpearsAndPikes,
        Halberds,
        Reapers,
        Whips,
        Bows,
        Greatbows,
        Crossbows,

        Staves,
        Flames,
        Talismans,
        SacredChimes,
        Shield = 25
    }

    public enum ItemType
    {
        None = 0,
        Weapon = 1,
        Armor = 2,
        Consumable = 3,
        Order = 4,
    }

    public enum EquimentItemCategory
    {
        Weapon,
        RightProjectile,
        Shield,
        LeftProjectile,
        Armor,
        Speicial,
        Ring,
        Item
    }
    #endregion

    #region Action

    public enum HitMotion
    {
        NormalHit,
        ShieldHit,
        CrouchingHit,
        CrouchShieldHit,
    }

    public enum MoveState
    {
        None,
        Walk,
        Run,
        Sprint,
        Falling,
        Jump
    }

    public enum ActionState
    {
        None,
        Shield,
        Charging,
        Reload,
        Invincible,
    }

    public enum UserAction
    {
        // Move
        Walk,
        Run_Forward,
        Run_Backward,
        Run_Left,
        Run_Right,
        Dash_BackStep_Roll,
        Jump,

        // Camera
        TiltCameraUp,
        TiltCameraDown,
        TiltCameraLeft_ChangeTargetLeft,
        TiltCameraRight_ChangeTargetRight,
        CameraReset_LockOn,

        // Switch Equipment
        SwitchSpells,
        SwitchQuickItems,
        SwitchRightHandWeapon,
        switchLeftHandWeapon,

        // Attack
        Attack_RightHand,
        StrongAttack_RightHand,
        Attack_LeftHand,
        StrongAttack_LeftHand,
        UseItem,
        Interact,
        TwoHandWeapon,

        // Key Bindings
        OpenMenu,
        OpenGestureMenu,
        MoveCurser_Up,
        MoveCurser_Down,
        MoveCurser_Right,
        MoveCurser_Left,
        Confirm,
        Cancel,
        SwitchTab_Left,
        SwitchTab_Right,
        Function1,
    }

    #endregion

    #region Other

    public enum OpenWhat
    {
        None,
        Inventory,
        Shop
    }

    public enum Layer
    {
        UI = 5,
        Player = 7,
        Monster = 8,
        Obstacle = 10,
        NPC = 11,
    }


    public enum CameraMode
    {
        Third,
        QuarterView,
        Aim
    }

    public enum Scene
    {
        Unknown = 0,
        Login = 1,
        Lobby = 2,
        Game = 3,
    }

    public enum Sound
    {
        Bgm = 0,
        Effect = 1,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Pressed,
        PointerDown,
        PointerUp,
    }

    public enum AnimationLayers
    {
        BaseLayer = 0,
        UpperLayer = 1,
    }

    public enum CursorType
    {
        None,
        Arrow,
        Hand,
        Look,
    }
    #endregion
}
