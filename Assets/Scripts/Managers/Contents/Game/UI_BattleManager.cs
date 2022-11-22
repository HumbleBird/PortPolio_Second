using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BattleManager 
{
    public UI_PlayerInfo UIPlayerInfo;

    public void Init()
    {
        GameObject go = Managers.Object.Find(1);
        Player player = go.GetComponent<Player>();
        UIPlayerInfo.SetInfo(player);
    }

    public void HitEvent()
    {
        UIPlayerInfo.HitEvent();
        // 화면 잠깐 빨갛게 페이드 인, 페이드 아웃하기
    }

    // UI
    //UI 관리
    public UI_SettingKey inputSettingKey;
}
