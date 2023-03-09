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

    Dictionary<string, UI_Popup> UIDic = new Dictionary<string, UI_Popup>();
    public T ShowAndClosePopup<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        T pop;

        // 처음 키는 거라면
        if (UIDic.ContainsKey(name) == false)
        {
            pop = Managers.UI.ShowPopupUI<T>();

            // 마우스
            if (UIDic.Count == 0)
                CursorController.MouseCurserLockOnOff(true);

            UIDic.Add(name, pop);
        }
        // 이미 켜져 있다면
        else
        {
            pop = (T)UIDic[name];

            Managers.UI.ClosePopupUI(pop);
            UIDic.Remove(name);

            // 마우스
            if (UIDic.Count == 0)
                CursorController.MouseCurserLockOnOff(false);
        }

        return pop;
    }
}
