using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_Inven_Item : UI_Base
{
    enum Texts
    {
        ItemCountText,
        ItemPriceText
    }

    enum Images
    {
        InventoryItemIcon,
        UsingItemCheckIcon
    }

    public TextMeshProUGUI m_iItemPrice { get; private set; }

    TextMeshProUGUI itemCount;
    Image itemIcon;
    Image itemUseIcon;

    public int m_iItemID { get; private set; }
    public int m_iSlot { get; private set; }
    public int m_iCount { get; private set; }
    public bool m_bEquipped { get; private set; }


    public override bool Init()
    {
        if (base.Init() == false)
            return false; 

        BindText(typeof(Texts));
        BindImage(typeof(Images));

        itemCount = GetText((int)Texts.ItemCountText);
        itemCount.gameObject.SetActive(false);

        m_iItemPrice = GetText((int)Texts.ItemPriceText);
        m_iItemPrice.enabled = false;

        itemIcon = GetImage((int)Images.InventoryItemIcon);

        itemIcon.gameObject.BindEvent(() =>
        {
            Item newitem = new Item(Define.ItemType.None);
            newitem.Id = m_iItemID;
            newitem.InventorySlot = m_iSlot;
            newitem.m_bEquipped = !m_bEquipped;

            newitem.eItemType = (ItemType)Managers.Table.m_Item.Get(newitem.Id).m_iItemType;

            Managers.Battle.EquipItem(Managers.Object.myPlayer, newitem);
        });

        itemIcon.gameObject.SetActive(false);

        itemUseIcon = GetImage((int)Images.UsingItemCheckIcon);
        itemUseIcon.gameObject.SetActive(false);
        
        return true;
    }

    public void SetItem(Item item)
    {
        if(item == null)
        {
            m_iItemID = 0;
            m_iSlot = 0;
            m_iCount = 0;
            m_bEquipped = false;

            itemIcon.gameObject.SetActive(false);
            itemCount.gameObject.SetActive(false);
        }
        else
        {
            m_iItemID = item.Id;
            m_iSlot = item.InventorySlot;
            m_iCount = item.Count;
            m_bEquipped = item.m_bEquipped;

            Table_Item.Info itemData = null;
            itemData = Managers.Table.m_Item.Get(m_iItemID);

            itemIcon.sprite = Managers.Resource.Load<Sprite>(itemData.m_sIconPath);
            itemIcon.gameObject.SetActive(true);

            if (item.eItemType == ItemType.Consumable)
            {
                itemCount.text = m_iCount.ToString();
                itemCount.gameObject.SetActive(true);
            }

            itemUseIcon.gameObject.SetActive(m_bEquipped);
        }
    }
}
