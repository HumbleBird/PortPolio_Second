using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BattleManager
{
    public UI_GameScene UIGameScene;

    public void Init()
    {
        UIGameScene = Managers.UI.SceneUI as UI_GameScene;
    }

    // �κ��丮
    public void InvenRefreshUI()
    {
        Init();
        UI_Inven UIInven = UIGameScene.UIInven;
        UIInven.RefreshUI();
    }

    // �÷��̾� HP/MP/Stamina
    public void StatRefershUI()
    {
        UI_PlayerInfo UIPlayerInfo = UIGameScene.UIPlayerInfo;
        UIPlayerInfo.RefreshUI();
    }

    // ���â
    public void EquipmentRefreshUI()
    {
        UI_Equipment UIEquipment = UIGameScene.UIEquipment;
        UIEquipment.RefreshUI();

        StatRefershUI();
    }

    public bool AreTheSlotsForThatItemFull(Item item)
    {
        UI_Equipment UIEquipment = UIGameScene.UIEquipment;
        return UIEquipment.AreTheSlotsForThatItemFull(item);
    }
}
