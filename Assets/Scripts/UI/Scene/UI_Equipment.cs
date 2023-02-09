using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_Equipment : UI_Base
{
    enum GameObjects
    {
        WeaponGrid,
        RightProjectileGrid,
        ShieldGrid,
        LeftProjectileGrid,
        ArmorGrid,
        SpeicialGrid,
        RingGrid,
        ItemGrid
    }

    int[] EquipmentInventoryCount = { 3, 2, 3, 2, 4, 1, 4, 10 };

    List<UI_Equipment_Item> AllGrid = new List<UI_Equipment_Item>();
    List<UI_Equipment_Item> WeaponGrid = new List<UI_Equipment_Item>();
    List<UI_Equipment_Item> RightProjectileGrid = new List<UI_Equipment_Item>();
    List<UI_Equipment_Item> ShieldGrid = new List<UI_Equipment_Item>();
    List<UI_Equipment_Item> LeftProjectileGrid = new List<UI_Equipment_Item>();
    List<UI_Equipment_Item> ArmorGrid = new List<UI_Equipment_Item>();
    List<UI_Equipment_Item> SpeicialGrid = new List<UI_Equipment_Item>();
    List<UI_Equipment_Item> RingGrid = new List<UI_Equipment_Item>();
    List<UI_Equipment_Item> ItemGrid = new List<UI_Equipment_Item>();

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));

        for (int i = 0; i < EquipmentInventoryCount.Length; i++)
        {
            CreateEquipmentInventory(EquipmentInventoryCount[i], i);
        }

        gameObject.SetActive(false);

        return true;
    }

    public void CreateEquipmentInventory(int count, int enumIndex)
    {
        GameObject gridPannel = GetObject(enumIndex);
        foreach (Transform child in gridPannel.transform)
            Managers.Resource.Destroy(child.gameObject);

        for (int i = 0; i < count; i++)
        {
            GameObject go = Managers.Resource.Instantiate("UI/Scene/UI_Equipment_Item", gridPannel.transform);
            UI_Equipment_Item item = go.GetOrAddComponent<UI_Equipment_Item>();

            item.eEquimentItemCategory = (EquimentItemCategory)enumIndex;

            AllGrid.Add(item);

            switch (item.eEquimentItemCategory)
            {
                case EquimentItemCategory.Weapon:
                    WeaponGrid.Add(item);
                    break;
                case EquimentItemCategory.RightProjectile:
                    RightProjectileGrid.Add(item);
                    break;
                case EquimentItemCategory.Shield:
                    ShieldGrid.Add(item);
                    break;
                case EquimentItemCategory.LeftProjectile:
                    LeftProjectileGrid.Add(item);
                    break;
                case EquimentItemCategory.Armor:
                    ArmorGrid.Add(item);
                    break;
                case EquimentItemCategory.Speicial:
                    SpeicialGrid.Add(item);
                    break;
                case EquimentItemCategory.Ring:
                    RingGrid.Add(item);
                    break;
                case EquimentItemCategory.Item:
                    ItemGrid.Add(item);
                    break;
            }
        }
    }

    public void RefreshUI()
    {
        if (_init == false)
            return;

        // 채워준다
        foreach (Item item in Managers.Inventory.m_Items)
        {
            if (item.m_bEquipped == false)
                continue;

            // 해당 아이템 카테고리에 차례대로 넣기
            if (item.eItemType == ItemType.Weapon)
            {
                foreach (UI_Equipment_Item itemInGrid in WeaponGrid)
                {
                    if (itemInGrid.m_bEquipped == false)
                    {
                        itemInGrid.EquipItem(item);
                        return;
                    }
                }
            }

            else if (item.eItemType == ItemType.Armor)
            {
                foreach (UI_Equipment_Item itemInGrid in ArmorGrid)
                {
                    if (itemInGrid.m_bEquipped == false)
                    {
                        itemInGrid.EquipItem(item);
                        return;
                    }
                }
            }

            else if (item.eItemType == ItemType.Consumable)
            {
                foreach (UI_Equipment_Item itemInGrid in ItemGrid)
                {
                    if (itemInGrid.m_bEquipped == false)
                    {
                        itemInGrid.EquipItem(item);
                        return;
                    }
                }
            }
        }
    }

    private void ItemAllReset()
    {
        foreach (UI_Equipment_Item item in AllGrid)
        {
            item.UnEquipItem();
        }

    }
}
