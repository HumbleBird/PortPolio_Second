using System;
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

    public void RefreshUI<T>(string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        T uibase = (T)UIGameScene.UIDic[name];
        uibase.RefreshUI();
    }

    public bool AreTheSlotsForThatItemFull(Item item)
    {
        UI_Equipment equipment = (UI_Equipment)UIGameScene.UIDic["UI_Equipment"];
        return equipment.AreTheSlotsForThatItemFull(item);
    }

    int m_iCount = 0;
    public T ShowAndCloseUI<T>(string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        T uibase = (T)UIGameScene.UIDic[name];
        bool b = uibase.gameObject.activeSelf;
        uibase.gameObject.SetActive(!b);
        uibase.RefreshUI();

        // UI�� ���� �Ŷ��
        if (b)
        {
            m_iCount--;

            if(m_iCount == 0)
                CursorController.MouseCurserLockOnOff(false);

        }
        // UI�� Ű�� �Ŷ��
        else
        {
            m_iCount++;

            if (m_iCount == 1)
                CursorController.MouseCurserLockOnOff(true);
        }

        return uibase;
    }
}
