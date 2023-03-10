using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameScene : UI_Scene
{
    public Dictionary<string, UI_Base> UIDic = new Dictionary<string, UI_Base>();

    public UI_PlayerInfo UIPlayerInfo { get; private set; }
    public UI_Equipment  UIEquipment  { get; private set; }
    public UI_Inven      UIInven      { get; private set; }
    public UI_Shop       UIShop       { get; private set; }
    public UI_SettingKey UISettingKey { get; private set; }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        UIPlayerInfo = GetComponentInChildren<UI_PlayerInfo>();
        UIEquipment  = GetComponentInChildren<UI_Equipment>();  
        UIInven      = GetComponentInChildren<UI_Inven>();      
        UIShop       = GetComponentInChildren<UI_Shop>();  
        UISettingKey = GetComponentInChildren<UI_SettingKey>();

        UIDic.Add(UIPlayerInfo.name, UIPlayerInfo);
        UIDic.Add(UIEquipment .name, UIEquipment );
        UIDic.Add(UIInven     .name, UIInven     );
        UIDic.Add(UIShop      .name, UIShop      );
        UIDic.Add(UISettingKey.name, UISettingKey);

        return true;
    }
}
