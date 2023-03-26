using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UI_Shop : UI_Base
{
    public UI_ShopInven UIShopInven { get; private set; }
    public UI_ItemDes UIItemDes { get; private set; }
    public UI_PlayerData UIPlayerData { get; private set; }
    public UI_BGGameInfo UIBGGameInfo { get; private set; }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        UIShopInven = GetComponentInChildren<UI_ShopInven>();
        UIItemDes = GetComponentInChildren<UI_ItemDes>();
        UIPlayerData = GetComponentInChildren<UI_PlayerData>();
        UIBGGameInfo = GetComponentInChildren<UI_BGGameInfo>();

        gameObject.SetActive(false);

        return true;
    }

    public override void RefreshUI()
    {
        base.RefreshUI();

        UIShopInven.RefreshUI();
        UIItemDes.RefreshUI();
        UIPlayerData.RefreshUI();
        UIBGGameInfo.RefreshUI();
    }
}
