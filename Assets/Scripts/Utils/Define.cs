﻿using System.Collections;
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
        Boots = 4,
        Gloves = 5,
        Ring = 6,
    }

    public enum WeaponType
    {
        None,
        Sword,
        Bow,
    }

    public enum ItemType
    {
        None = 0,
        Weapon = 1,
        Armor = 2,
        Consumable = 3
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
        Monster = 8,
        Ground = 9,
        Block = 10,
    }


    public enum CameraMode
    {
        Third,
        QuarterView,
        Anim
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

    public enum AnimationLayers
    {
        BaseLayer = 0,
        UpperLayer = 1,
    }
    #endregion
}
