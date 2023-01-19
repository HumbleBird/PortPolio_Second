using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BattleManager
{
    public UI_PlayerInfo UIPlayerInfo;
    public UI_Inven      UIInven;
    public UI_SettingKey UISetting;

    public void StatUIRefersh()
    {
        UIPlayerInfo.RefreshUI();
    }
}
