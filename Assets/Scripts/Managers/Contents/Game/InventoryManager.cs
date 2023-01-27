using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager 
{
    public Dictionary<int, Item> m_dicItem { get; } = new Dictionary<int, Item>();

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
        // 인벤토리의 최대 수까지
        for (int slot = 0; slot < 25; slot++)
        {
            if (Util.TryFirstOrDefault<Item>(m_dicItem.Values, out Item item) == false)
            {
                return slot;
            }
        }

        return null;
    }
}
