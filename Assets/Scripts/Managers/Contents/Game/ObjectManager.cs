using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ObjectManager
{
	// 추후에 서버 붙으면 자주 이용할 오브젝트 매니저

	Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();
	Dictionary<int, Item> _items = new Dictionary<int, Item>();

	public void Add(int id, GameObject go)
	{
		_objects.Add(id, go);
	}

	public void Remove(int id)
	{
		_objects.Remove(id);
	}

	public GameObject Find(int id)
    {
		GameObject obj = null;
		_objects.TryGetValue(id, out obj);
		if (obj == null)
			return null;

		return obj;
    }

    public GameObject Find(Func<GameObject, bool> condition)
    {
        foreach (GameObject obj in _objects.Values)
        {
            if (condition.Invoke(obj))
                return obj;
        }

        return null;
    }

    public void Clear()
	{
		_objects.Clear();
	}


    // 아이템
    public Item MakeItem(int id)
    {
        Item item = null;

        Table_Item.Info itemInfo = Managers.Table.m_Item.Get(id);
        if (itemInfo == null)
            return null;

        switch (itemInfo.m_iItemType)
        {
            case (int)ItemType.Weapon:
                item = new Weapon(itemInfo.m_nID);
                break;
            case (int)ItemType.Armor:
                item = new Armor(itemInfo.m_nID);
                break;
            case (int)ItemType.Consumable:
                item = new Consumable(itemInfo.m_nID);
                break;
            default:
                break;
        }

        return item;
    }
}
