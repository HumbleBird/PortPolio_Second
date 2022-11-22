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
        // ȭ�� ��� ������ ���̵� ��, ���̵� �ƿ��ϱ�
    }

    // UI
    //UI ����
    public UI_SettingKey inputSettingKey;
}
