using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static Define;

public class UI_ShopInven : UI_Base
{
    // <���� 1����>
    // 1. ���̺��� �Ǹ��� ������ ������ ���� �����´�. v
    // 2. ������ �� �� Ŭ�� ����â ���, �� ���¿��� �� Ŭ���� ���� ���� ����â�� ���
    // 3. ���� �����Ѵٸ� ������ ���ݸ�ŭ ��带 �����ϰ� �÷��̾��� ������ �κ��� �������� �߰��Ѵ�.

    // <���� 2����>
    // ������ �������� ������
    // ���Ž� ������ ���� ���� ����. ���� �ð��� ������ �ٽ� ä���

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

        // ������ ĭ ����
        GameObject gridPannel = Get<GameObject>((int)GameObjects.ItemGridPannel);
        foreach (Transform child in gridPannel.transform)
            Managers.Resource.Destroy(child.gameObject);

        for (int i = 0; i < Managers.Table.m_Shop.m_Dictionary.Count; i++)
        {
            GameObject go = Managers.Resource.Instantiate("UI/SubItem/UI_Inven_Item", gridPannel.transform);
            UI_Inven_Item item = go.GetOrAddComponent<UI_Inven_Item>();

            item.eOpenWhat = OpenWhat.Shop;

            Items.Add(item);
        }

        _init = true;
        RefreshUI();

        return true;
    }

    public override void RefreshUI()
    {
        base.RefreshUI();

        List<Item> items = new List<Item>();
        int slot = 0;

        // 1. ������ ���̺� ���� ��� ��������
        foreach (Table_Shop.Info itemInfo in Managers.Table.m_Shop.m_Dictionary.Values)
        {
            Table_Item.Info info = Managers.Table.m_Item.Get(itemInfo.m_iItemIdx);
            Item item = Item.MakeItem(info);
            item.InventorySlot = slot;
            item.Count = itemInfo.m_iCount;
            item.m_iPrice = itemInfo.m_iPrice;
            slot++;
            items.Add(item);
        }

        items.Sort((left, right) => { return left.InventorySlot - right.InventorySlot; });

        // 2. ���� �κ��� ���� �ֱ�
        foreach (Item item in items)
        {
            if (item.Count < 0 )
                continue;

            Items[item.InventorySlot].SetItem(item);
            Items[item.InventorySlot].Shop(true); // ���� ���� ����
        }
    }
}
