using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UI_Shop : UI_Base
{
    public UI_ShopInven UIShopInven { get; set; }
    public UI_ItemDes UIItemDes { get; set; }
    public UI_CharacterData UICharacterData { get; set; }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        UIShopInven = GetComponentInChildren<UI_ShopInven>();
        UIItemDes = GetComponentInChildren<UI_ItemDes>();
        UICharacterData = GetComponentInChildren<UI_CharacterData>();

        gameObject.SetActive(false);

        return true;
    }

    public override void RefreshUI()
    {
        base.RefreshUI();

        UIShopInven.RefreshUI();
        UIItemDes.RefreshUI();

        UICharacterData.RefreshUI();
    }
}
