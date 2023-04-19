using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentManager
{
    public List<Item> m_ListAllItems { get; } = new List<Item>();
    public Weapon[] m_ListWeaponInRightHandSlots { get;  set; } = new Weapon[3];
    public Weapon[] m_ListWeaponInLeftHandSlots  { get;  set; } = new Weapon[3];

    public void Add(int index, Item item)
    {
        switch (item.eItemType)
        {
            case Define.ItemType.None:
                break;
            case Define.ItemType.Weapon:
                m_ListWeaponInRightHandSlots[index] = (Weapon)item;
                break;
            case Define.ItemType.Armor:
                break;
            case Define.ItemType.Consumable:
                break;
            case Define.ItemType.Order:
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
            case Define.ItemType.None:
                break;
            case Define.ItemType.Weapon:
                m_ListWeaponInRightHandSlots[index] = null;
                break;
            case Define.ItemType.Armor:
                break;
            case Define.ItemType.Consumable:
                break;
            case Define.ItemType.Order:
                break;
            default:
                break;
        }

        m_ListAllItems.Remove(item);
    }

    public Item Find(Func<Item, bool> condition)
    {
        foreach (Item item in m_ListWeaponInRightHandSlots)
        {
            if (condition.Invoke(item))
                return item;
        }

        return null;
    }

    public int? GetEmptySlot()
    {
        for (int slot = 0; slot < 3; slot++)
        {
            if (m_ListWeaponInRightHandSlots[slot] == null)
                return slot;
        }

        return null;
    }
}
