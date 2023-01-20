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

    public override bool Init()
    {
        if (base.Init() == false)
            return false; 

        BindText(typeof(Texts));
        BindImage(typeof(Images));

        itemCount = GetText((int)Texts.ItemCountText);
        itemCount.gameObject.SetActive(false);

        itemIcon = GetImage((int)Images.InventoryItemIcon);
        itemIcon.gameObject.SetActive(false);

        itemUseIcon = GetImage((int)Images.UsingItemCheckIcon);
        itemUseIcon.gameObject.SetActive(false);
        
        return true;
    }

    public void SetItem(int itemId, int count)
    {
        Table_Item.Info itemData = null;
        itemData = Managers.Table.m_Item.Get(itemId);
        itemIcon.sprite = Managers.Resource.Load<Sprite>(itemData.m_sIconPath);
        itemIcon.gameObject.SetActive(true);
        itemCount.text = count.ToString();
        itemCount.gameObject.SetActive(true);
    }
}
