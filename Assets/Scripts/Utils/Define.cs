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
        Archer = 2,
        Wizard = 3
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

    #region Battle

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
        None = 0,
        Sword = 1,
        Bow = 2,
    }

    public enum ItemType
    {
        None = 0,
        Weapon = 1,
        Armor = 2,
        Consumable = 3,
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
        Crouch,
    }

    public enum AttackCollider
    {
        None,
        Weapon,
        CharacterFront,
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
        //Battle
        BasicAttack,
        StrongAttack,
        Kick,
        Shield,

        //Action
        //Jump,
        Roll,
        Crouch,

        // UI
        UI_Inventory,
        UI_Equipment,
        UI_Status,
        UI_Skill,
        UI_Setting
    }

    #endregion

    #region Other

    public enum Layer
    {
        UI = 5,
        Monster = 8,
        Ground = 9,
        Block = 10,
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
