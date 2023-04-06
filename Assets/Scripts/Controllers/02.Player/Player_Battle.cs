﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Player : Character
{
	protected bool m_bNextAttack = false;

	protected override void AttackEvent(int id)
    {
        base.AttackEvent(id);

		eState = CreatureState.Skill;

		// 스테미너 감소
		float newStemina = m_Stat.m_fStemina - 10;
		SetStemina(newStemina);
	}

    #region Weapon
    
    void WeaponInit()
    {
        ItemSoket[] weaponHolderSlots = GetComponentsInChildren<ItemSoket>();
        foreach (ItemSoket weaponslot in weaponHolderSlots)
        {
            if (weaponslot.isLeftHandSlot)
            {
                m_leftHandSlot = weaponslot;
            }
            else if (weaponslot.isRightHandSlot)
            {
                m_RightHandSlot = weaponslot;
            }
        }
    }

    public void LoadWeaponOnSlot(Weapon weaponItem, bool isLeft)
    {
        if(isLeft)
        {
            m_leftHandSlot.LoadWeaponModel(weaponItem);
        }
        else
        {
            m_RightHandSlot.LoadWeaponModel(weaponItem);

        }
    }    
    
    public void UnLoadWeaponOnSlot(bool isLeft)
    {
        if(isLeft)
        {
            m_leftHandSlot.UnloadWeapon();
        }
        else
        {
            m_RightHandSlot.UnloadWeapon();

        }
    }

    #endregion

    #region PlayerAction
    public void Roll()
    {
        string animName = "Run To Roll";

        PlayAnimation(animName);
        float time = GetAnimationTime(animName);
        Stop(time * 0.8f);
        eActionState = ActionState.Invincible;
    }

    public void BackStep()
    {
        string animName = "BackStep";

        PlayAnimation(animName);
        float time = GetAnimationTime(animName);
        Stop(time * 0.8f);
    }

    #endregion
}
