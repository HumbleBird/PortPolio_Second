using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UI_Inven : UI_Base
{
    public UI_InvenMain UIInvenMain { get; set; }
    public UI_ItemDes UIItemDes { get; set; }
    public UI_PlayerData UIPlayerData { get; set; }

    Player _player;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        UIInvenMain = GetComponentInChildren<UI_InvenMain>();
        UIItemDes = GetComponentInChildren<UI_ItemDes>();
        UIPlayerData = GetComponentInChildren<UI_PlayerData>();

        gameObject.SetActive(false);

        return true;
    }


    public void RefreshUI()
    {
        if (_init == false)
            return;

        UIInvenMain.RefreshUI();
        UIItemDes.RefreshUI();

        // Temp
        UIPlayerData.SetInfo(Managers.Object.Find(1));
        UIPlayerData.RefreshUI();

    }
}
