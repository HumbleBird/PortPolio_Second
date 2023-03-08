using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameScene : UI_Scene
{
    public UI_PlayerInfo UIPlayerInfo { get; private set; }

    public UI_Inven UIInven { get; private set; }
    public UI_SettingKey UISetting { get; private set; }
    public UI_Equipment UIEquipment { get; private set; }
    public UI_Shop UIShop { get; private set; }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        UIPlayerInfo = GetComponentInChildren<UI_PlayerInfo>();
        UIInven = GetComponentInChildren<UI_Inven>();
        UISetting = GetComponentInChildren<UI_SettingKey>();
        UIEquipment = GetComponentInChildren<UI_Equipment>();
        UIShop = GetComponentInChildren<UI_Shop>();

        UISetting.Init();

        return true;
    }
}
