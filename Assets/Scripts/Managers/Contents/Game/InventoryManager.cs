using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager 
{
    public Dictionary<int, Item> m_dicItem { get; } = new Dictionary<int, Item>();
    public int m_iSlotCountMax { get; private set; } = 25;

    public void Add(Item item)
    {
        m_dicItem.Add(item.Id, item);
    }

    public Item Get(int itemID)
    {
        Item item = null;
        m_dicItem.TryGetValue(itemID, out item);
        return item;
    }

    public Item Find(Func<Item, bool> condition)
    {
        foreach (Item item in m_dicItem.Values)
        {
            if (condition.Invoke(item))
                return item;
        }

        return null;
    }

    public void Clear()
    {
        m_dicItem.Clear();
    }

    public int? GetEmptySlot()
    {
        for (int slot = 0; slot < Managers.Inventory.m_iSlotCountMax; slot++)
        {
            Item item = m_dicItem.Values.FirstOrDefault(i => i.Slot == slot);
            if (item == null)
                return slot;
        }

        return null;
    }
}
