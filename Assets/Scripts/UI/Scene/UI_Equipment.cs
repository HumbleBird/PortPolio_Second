using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using System.Linq;
using TMPro;

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

    enum Texts
    {
        EquipmentPartsText,
        EquipmentNameText
    }

    public TextMeshProUGUI equipmentPartsText;
    public TextMeshProUGUI equipmentNameText;
    bool m_bStartClick = false;

    int[] EquipmentInventoryCount = { 3, 2, 3, 2, 4, 1, 4, 10 };

    #region Grid
    List<UI_Equipment_Item> AllGrid = new List<UI_Equipment_Item>();
    List<UI_Equipment_Item> WeaponGrid = new List<UI_Equipment_Item>();
    List<UI_Equipment_Item> RightProjectileGrid = new List<UI_Equipment_Item>();
    List<UI_Equipment_Item> ShieldGrid = new List<UI_Equipment_Item>();
    List<UI_Equipment_Item> LeftProjectileGrid = new List<UI_Equipment_Item>();
    List<UI_Equipment_Item> ArmorGrid = new List<UI_Equipment_Item>();
    List<UI_Equipment_Item> SpeicialGrid = new List<UI_Equipment_Item>();
    List<UI_Equipment_Item> RingGrid = new List<UI_Equipment_Item>();
    List<UI_Equipment_Item> ItemGrid = new List<UI_Equipment_Item>();
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));

        equipmentPartsText = GetText((int)Texts.EquipmentPartsText);
        equipmentNameText = GetText((int)Texts.EquipmentNameText);

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
            item.m_iSlot = i;

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

    public override void RefreshUI()
    {
        base.RefreshUI();

        // 장비칸에 아이템 넣기
        foreach (Item item in Managers.Inventory.m_Items)
        {
            // 아이템 해제
            if (item.m_bEquipped == false)
            {
                // 이 아이템이 등록 된 칸을 찾아서 아이템 해제하기
                if (item.eItemType == ItemType.Weapon)
                {
                    foreach (UI_Equipment_Item itemInGrid in WeaponGrid)
                    {
                        if(itemInGrid.m_iSlot == item.EquipmentSlot)
                        {
                            itemInGrid.UnEquipItem();
                            item.EquipmentSlot = -1;
                        }
                    }
                }

                else if (item.eItemType == ItemType.Armor)
                {
                    foreach (UI_Equipment_Item itemInGrid in ArmorGrid)
                    {
                        if (itemInGrid.m_iSlot == item.EquipmentSlot)
                        {
                            itemInGrid.UnEquipItem();
                            item.EquipmentSlot = -1;
                        }
                    }
                }

                else if (item.eItemType == ItemType.Consumable)
                {
                    foreach (UI_Equipment_Item itemInGrid in ItemGrid)
                    {
                        if (itemInGrid.m_iSlot == item.EquipmentSlot)
                        {
                            itemInGrid.UnEquipItem();
                            item.EquipmentSlot = -1;
                        }
                    }
                }
                continue;
            }

            // 아이템 장착
            if (item.eItemType == ItemType.Weapon)
            {
                foreach (UI_Equipment_Item itemInGrid in WeaponGrid)
                {
                    if (itemInGrid.m_bEquipped == false && item.EquipmentSlot == -1)
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
                    if (itemInGrid.m_bEquipped == false && item.EquipmentSlot == -1)
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
                    if (itemInGrid.m_bEquipped == false && item.EquipmentSlot == -1)
                    {
                        itemInGrid.EquipItem(item);
                        return;
                    }
                }
            }
        }

        // 처음 장비창을 열었다면
        if (m_bStartClick == false)
        {
            foreach (var item in AllGrid)
            {
                if (item.m_iItemId != -1)
                {
                    equipmentPartsText.text = "Get";
                    equipmentNameText.text = item.m_sName;
                }
                else
                {
                    equipmentPartsText.text = "";
                    equipmentNameText.text = "";
                }
            }
        }

        Managers.UIBattle.RefreshUI(Managers.UIBattle.UIGameScene.UIPlayerInfo);

    }

    private void ItemAllReset()
    {
        foreach (UI_Equipment_Item item in AllGrid)
        {
            item.UnEquipItem();
        }
    }

    public bool AreTheSlotsForThatItemFull (Item item)
    {
        if (item.eItemType == ItemType.Weapon)
        {
            UI_Equipment_Item equipped = WeaponGrid.FirstOrDefault(i => i.m_bEquipped == false);
            // 비워진 슬롯이 없다. 장비 창이 꽉 찼다
            if (equipped == false)
                return true;
        }

        else if (item.eItemType == ItemType.Armor)
        {
            UI_Equipment_Item equipped = ArmorGrid.FirstOrDefault(i => i.m_bEquipped == false);
            if (equipped == false)
                return true;
        }

        else if (item.eItemType == ItemType.Consumable)
        {
            UI_Equipment_Item equipped = ItemGrid.FirstOrDefault(i => i.m_bEquipped == false);
            if (equipped == false)
                return true;
        }

        return false;
    }
}
