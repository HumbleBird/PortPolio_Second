using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BattleManager : MonoBehaviour
{
    public UI_PlayerInfo UIPlayerInfo;
    public UI_Inven      UIInven;
    public UI_SettingKey UISetting;

    public void Init()
    {
        UIInit();
    }

    public void UIInit()
    {
        GameObject go = Managers.Object.Find(1);
        Player player = go.GetComponent<Player>();

        // UI ·Îµå
        UIPlayerInfo = Managers.UI.ShowSceneUI<UI_PlayerInfo>();
        UISetting = Managers.UI.ShowPopupUI<UI_SettingKey>();
        UIInven = Managers.UI.ShowPopupUI<UI_Inven>();

        UIPlayerInfo.SetInfo(player);
        UISetting.Init();
    }

    public void StatUIRefersh()
    {
        UIPlayerInfo.RefreshUI();
    }
}
