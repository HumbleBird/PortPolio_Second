using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

public class UI_InvenMain : UI_Base
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

        for (int i = 0; i < Managers.Inventory.m_iSlotCountMax; i++)
        {
            GameObject go = Managers.Resource.Instantiate("UI/SubItem/UI_Inven_Item", gridPannel.transform);
            UI_Inven_Item item = go.GetOrAddComponent<UI_Inven_Item>();

            item.eOpenWhat = OpenWhat.Inventory;

            Items.Add(item);
        }

        b = true;

        return true;
    }

    bool b = false;

    public override void RefreshUI()
    {
        base.RefreshUI();

        if(!b)
        {
            Managers.UIBattle.ShowAndCloseUI<UI_Inven>();
            Managers.UIBattle.ShowAndCloseUI<UI_Inven>();
        }

        // 아이템 창 아이템
        List<Item> items = Managers.Inventory.m_Items;
        items.Sort((left, right) => { return left.InventorySlot - right.InventorySlot; });

        foreach (Item item in items)
        {
            if (item.Count < 0 || item.Count >= Managers.Inventory.m_iSlotCountMax)
                continue;

            Items[item.InventorySlot].SetItem(item);
            Items[item.InventorySlot].Shop(false);
        }
    }
}
