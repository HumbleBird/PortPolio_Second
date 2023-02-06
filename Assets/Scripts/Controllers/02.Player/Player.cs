using System;
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
        i.Slot == equipItem.Slot);

        if (item == null)
            return;

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

        {
            item.m_bEquipped = equipItem.m_bEquipped;

            Managers.UIBattle.UIInvenRefresh();
        }

        RefreshAdditionalStat();
    }

    public void UseItem(Item item)
    {
        Debug.Log("아이템 사용");
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
                case ItemType.None:
                    break;
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
