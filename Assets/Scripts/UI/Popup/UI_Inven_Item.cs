using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetText((int)Texts.ItemCountText).gameObject.SetActive(false);
        GetImage((int)Images.UsingItemCheckIcon).gameObject.SetActive(false);
        
        return true;
    }

    public void RefreshUI()
    {
        if (_init == false)
            return;


    }
}
