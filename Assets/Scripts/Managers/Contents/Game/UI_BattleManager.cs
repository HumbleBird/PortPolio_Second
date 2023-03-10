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

    public void RefreshPopupUI<T>(string name = null)
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        UI_Popup popup = Managers.Resource.Load<UI_Popup>($"Prefabs/UI/Popup/{name}");

        popup.RefreshUI();
    }

    

    public IEnumerator DelegateShowAndClose(Action action)
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                action.Invoke();
                yield break;
            }

            yield return null;
        }

    }

    public bool AreTheSlotsForThatItemFull(Item item)
    {
        UI_Equipment equipment = Managers.Resource.Load<UI_Equipment>("Prefabs/UI/Popup/UI_Equipment");
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

        // UI를 끄는 거라면
        if (b)
        {
            m_iCount--;

            if(m_iCount == 0)
                CursorController.MouseCurserLockOnOff(false);

        }
        // UI를 키는 거라면
        else
        {
            m_iCount++;

            if (m_iCount == 1)
                CursorController.MouseCurserLockOnOff(true);
        }

        return uibase;
    }
}
