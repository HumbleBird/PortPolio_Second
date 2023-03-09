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

        UI_SettingKey popup =Managers.UI.ShowPopupUI<UI_SettingKey>();
        popup.ClosePopupUI();

        Managers.InputKey.Init();
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
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                action.Invoke();
                break;
            }

            yield return null;
        }

    }

    public bool AreTheSlotsForThatItemFull(Item item)
    {
        UI_Equipment equipment = Managers.Resource.Load<UI_Equipment>("Prefabs/UI/Popup/UI_Equipment");
        return equipment.AreTheSlotsForThatItemFull(item);
    }
}
