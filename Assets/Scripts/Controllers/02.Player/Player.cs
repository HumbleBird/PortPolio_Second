﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Player : Character
{
    public int m_iWeaponDamage { get; private set; }
    public int m_iArmorDefence { get; private set; }

    public override float m_TotalAttack { get { return m_strStat.m_fAtk + m_iWeaponDamage; } }
    public override float m_TotalDefence { get { return m_strStat.m_fDef + m_iArmorDefence; } }

    protected Coroutine cStaminaGraduallyFillingUp;

    protected override void Init()
    {
        base.Init();

        tag = "Player";

        cStaminaGraduallyFillingUp = StartCoroutine(StaminaGraduallyFillingUp());
    }

    protected override void UpdateSkill()
    {
        base.UpdateSkill();

        cCheckNextAttack = StartCoroutine(CheckNextAttack());
    }

    public void EquipItem(Item equipItem)
    {
        if (equipItem == null)
            return;

        Item item = Managers.Inventory.Find(i =>
        i.Id == equipItem.Id &&
        i.InventorySlot == equipItem.InventorySlot);

        if (item == null)
            return;

        // 아이템 해제

        // 해당 아이템의 카테고리의 칸이 전부 꽉차면 장착 x
        // TODO 아이템 장착 전 장비창을 열어 아이템 장착
        // -> 빈 칸에 아이템을 장착을, 아이템이 이미 장착 중이라면 새롭게 장착을
        if (Managers.UIBattle.AreTheSlotsForThatItemFull(item) && item.m_bEquipped == false)
        {
            Debug.Log("장비 창의 빈 칸이 없습니다. 장비 창의 아이템을 비워주세요.");
            return;
        }

        {
            item.m_bEquipped = equipItem.m_bEquipped;

            

            Managers.UIBattle.UIInvenRefresh();
        }

        RefreshAdditionalStat();
    }

    public void UnEquipItem(Item equipItem)
    {
        Item item = null;
        // 장착 해제
        if (equipItem.m_bEquipped)
        {
            Item unequipItem = null;

            if (item.eItemType == ItemType.Weapon)
            {
                unequipItem = Managers.Inventory.Find(i =>
                i.m_bEquipped && i.eItemType == ItemType.Weapon);
            }
            else if (item.eItemType == ItemType.Armor)
            {
                ArmorType armorType = ((Armor)item).eArmorType;

                unequipItem = Managers.Inventory.Find(i =>
                      i.m_bEquipped && i.eItemType == ItemType.Armor
                         && ((Armor)i).eArmorType == armorType);
            }

            if (unequipItem != null)
            {
                unequipItem.m_bEquipped = false;

                Managers.UIBattle.UIInvenRefresh();
            }
        }
    }

    // 소모품 아이템
    public void UseItem(Item item)
    {
        Debug.Log("아이템 사용");

        // TODO 아이템 효과에 따라
        // 대분류 : consumable (포션, 퀘스트 아이템 등)
        // 분류 : potionEffect (포션 종류에 따라)
    }

    public void RefreshAdditionalStat()
    {
        m_iWeaponDamage = 0;
        m_iArmorDefence = 0;

        foreach (Item item in Managers.Inventory.m_Items)
        {
            if (item.m_bEquipped == false)
                continue;

            switch (item.eItemType)
            {
                case ItemType.Weapon:
                    m_iWeaponDamage += ((Weapon)item).Damage;
                    break;
                case ItemType.Armor:
                    m_iArmorDefence += ((Armor)item).Defence;
                    break;
                case ItemType.Consumable:
                    break;
                default:
                    break;
            }
        }
    }
}
