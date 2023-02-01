﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum CharacterJob
    {
        None,
        Warior,
        Archor,
        Wizard
    }

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

    public enum CharacterClass
    {
        None = 0,
        Warior = 1,
        Archer = 2,
        Wizard = 3
    }

    public enum ObjectType
    {
        None,
        Player,
        Monster,
        Projectile
    }

    public enum Layers
    {
        BaseLayer = 0,
        UpperLayer = 1,
    }

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
