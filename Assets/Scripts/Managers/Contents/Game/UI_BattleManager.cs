using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BattleManager : MonoBehaviour
{
    public UI_PlayerInfo UIPlayerInfo;
    public UI_SettingKey UISetting;

    public void Init()
    {
        GameObject go = Managers.Object.Find(1);
        Player player = go.GetComponent<Player>();
        UIPlayerInfo.SetInfo(player);
        UISetting.Init();
        Managers.InputKey.Init();
    }

    public void StatUIRefersh()
    {
        UIPlayerInfo.RefreshUI();
    }
}
