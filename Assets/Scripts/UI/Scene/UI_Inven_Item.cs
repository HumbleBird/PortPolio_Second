using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inven_Item : UI_Base
{
    enum Texts
    {
        ItemCountText
    }

    enum Images
    {
        InventoryItemIcon,
        UsingItemCheckIcon
    }

    TextMeshProUGUI itemCount;
    Image itemIcon;
    Image itemUseIcon;

    public int m_iItemID { get; private set; }
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

        itemIcon = GetImage((int)Images.InventoryItemIcon);

        itemIcon.gameObject.BindEvent(() =>
        {
            Debug.Log("Click Item");
            Item newitem = new Item(Define.ItemType.None);
            newitem.Id = m_iItemID;
            newitem.m_bEquipped = !m_bEquipped;
            Managers.Battle.EquipItem(Managers.Battle.myPlayer, newitem);
        });

        itemIcon.gameObject.SetActive(false);

        itemUseIcon = GetImage((int)Images.UsingItemCheckIcon);
        itemUseIcon.gameObject.SetActive(false);
        
        return true;
    }

    public void SetItem(Item item)
    {
        m_iItemID = item.Id;
        m_iCount = item.Count;
        m_bEquipped = item.m_bEquipped;

        Table_Item.Info itemData = null;
        itemData = Managers.Table.m_Item.Get(m_iItemID);

        itemIcon.sprite = Managers.Resource.Load<Sprite>(itemData.m_sIconPath);
        itemIcon.gameObject.SetActive(true);

        itemCount.text = m_iCount.ToString();
        itemCount.gameObject.SetActive(true);
    }

    public void ItemClick()
    {

    }
}
