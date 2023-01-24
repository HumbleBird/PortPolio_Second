using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameScene : UI_Scene
{
    public UI_PlayerInfo UIPlayerInfo { get; set; }
    public UI_Inven UIInven { get; set; }
    public UI_SettingKey UISetting { get; set; }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        UIPlayerInfo = GetComponentInChildren<UI_PlayerInfo>();
        UIInven = GetComponentInChildren<UI_Inven>();
        UISetting = GetComponentInChildren<UI_SettingKey>();

        UISetting.Init();

        return true;
    }
}
