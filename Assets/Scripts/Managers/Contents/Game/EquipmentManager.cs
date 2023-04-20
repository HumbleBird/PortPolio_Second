using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

public class EquipmentManager
{
    public List<Item> m_ListAllItems { get; } = new List<Item>();
    public Weapon[] m_ListWeaponInRightHandSlots { get;  set; } = new Weapon[3];
    public Weapon[] m_ListWeaponInLeftHandSlots  { get;  set; } = new Weapon[3];

    public void Add(int index, Item item)
    {
        switch (item.eItemType)
        {
            case ItemType.None:
                break;
            case ItemType.Weapon:
                Weapon weapon = (Weapon)item;
                if(weapon.eWeaponType != WeaponType.Shield)
                    m_ListWeaponInRightHandSlots[index] = (Weapon)item;
                else
                    m_ListWeaponInLeftHandSlots[index] = (Weapon)item;
                break;
            case ItemType.Armor:
                break;
            case ItemType.Consumable:
                break;
            case ItemType.Order:
                break;
            default:
                break;
        }

        m_ListAllItems.Add(item);
    }

    public void Remove(int index, Item item)
    {
        switch (item.eItemType)
        {
            case ItemType.None:
                break;
            case ItemType.Weapon:
                Weapon weapon = (Weapon)item;
                if (weapon.eWeaponType != WeaponType.Shield)
                    m_ListWeaponInRightHandSlots[index] = null;
                else
                    m_ListWeaponInLeftHandSlots[index] = null;
                break;
            case ItemType.Armor:
                break;
            case ItemType.Consumable:
                break;
            case ItemType.Order:
                break;
            default:
                break;
        }

        m_ListAllItems.Remove(item);
    }

    public int? GetEmptySlot(Item item)
    {
        switch (item.eItemType)
        {
            case ItemType.None:
                break;
            case ItemType.Weapon:
                Weapon weapon = (Weapon)item;
                if (weapon.eWeaponType != WeaponType.Shield)
                {
                    for (int slot = 0; slot < 3; slot++)
                    {
                        if (m_ListWeaponInRightHandSlots[slot] == null)
                            return slot;
                    }
                }
                else
                {
                    for (int slot = 0; slot < 3; slot++)
                    {
                        if (m_ListWeaponInLeftHandSlots[slot] == null)
                            return slot;
                    }
                }
                break;
            case ItemType.Armor:
                break;
            case ItemType.Consumable:
                break;
            case ItemType.Order:
                break;
            default:
                break;
        }

        return null;
    }
}
