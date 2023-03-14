using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static Define;

public class UI_ShopInven : UI_Base
{
    // <구현 1순위>
    // 1. 테이블에서 판매할 아이템 정보를 전부 가져온다. v
    // 2. 아이템 한 번 클릭 설명창 띄움, 이 상태에서 또 클릭시 구매 여부 선택창을 띄움
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

        // 1. 데이터 테이블 정보 모두 가져오기
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

        // 2. 상점 인벤에 정보 넣기
        foreach (Item item in items)
        {
            if (item.Count < 0 )
                continue;

            Items[item.InventorySlot].SetItem(item);
            Items[item.InventorySlot].Shop(true); // 가격 정보 열기
        }
    }
}
