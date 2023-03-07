using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UI_ShopInven : UI_Base
{
    // <구현 1순위>
    // 1. 테이블에서 판매할 아이템 정보를 전부 가져온다.
    // 2. 아이템에 마우스를 가져다대면 아이템 정보창에 정보를 띄우고, 클릭하면 아이템을 살지 말지 선택창을 띄운다.
    // 3. 살지 선택한다면 지정된 가격만큼 골드를 차감하고 플레이어의 아이템 인벤에 아이템을 추가한다.

    // <구현 2순위>
    // 아이템 종류별로 나누기
    // 구매시 아이템 지정 갯수 차감. 이후 시간이 지나면 다시 채우기

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
            GameObject go = Managers.Resource.Instantiate("UI/Scene/UI_Inven_Item", gridPannel.transform);
            UI_Inven_Item item = go.GetOrAddComponent<UI_Inven_Item>();
            Items.Add(item);
        }

        _init = true;
        RefreshUI();

        return true;
    }

    public void RefreshUI()
    {
        if (_init == false)
            return;

        // 아이템 창 아이템
        //List<Item> items = Managers.Inventory.m_dicItem.Values.ToList();
        List<Item> items = Managers.Inventory.m_Items.ToList();
        items.Sort((left, right) => { return left.InventorySlot - right.InventorySlot; });

        foreach (Item item in items)
        {
            if (item.Count < 0 || item.Count >= Managers.Inventory.m_iSlotCountMax)
                continue;

            Items[item.InventorySlot].SetItem(item);
        }
    }
}
