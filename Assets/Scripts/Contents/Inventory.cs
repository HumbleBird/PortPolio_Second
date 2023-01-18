using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Inventory
{
    Dictionary<int, Item> m_dicItem = new Dictionary<int, Item>();

    public void Add(Item item)
    {
        m_dicItem.Add(0, item);
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

}
