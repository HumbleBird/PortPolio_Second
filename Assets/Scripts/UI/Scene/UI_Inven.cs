using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Inven : UI_Base
{
    public List<UI_Inven_Item> Items { get; set; } = new List<UI_Inven_Item>();

    enum GameObjects
    {
        ItemGridPannel
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));

        // 아이템 칸 생성
        GameObject gridPannel = Get<GameObject>((int)GameObjects.ItemGridPannel);
        foreach (Transform child in gridPannel.transform)
            Managers.Resource.Destroy(child.gameObject);

        int InitItemPannelCount = 25;
        for (int i = 0; i < InitItemPannelCount; i++)
        {
            GameObject go = Managers.Resource.Instantiate("UI/Scene/UI_Inven_Item", gridPannel.transform);
            UI_Inven_Item item = go.GetOrAddComponent<UI_Inven_Item>();
            Items.Add(item);
        }

        return true;
    }

    public void RefreshUI()
    {
        if (_init == false)
            return;

        List<Item> items = Managers.Inventory.m_dicItem.Values.ToList();
        items.Sort((left, right) => { return left.Slot - right.Slot; });

        foreach (Item item in items)
        {
            if (item.Count < 0 || item.Count >= 20)
                continue;

            Items[item.Slot].SetItem(item.Id, item.Count);
        }
    }
}
