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
}
