using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BattleManager
{
    public UI_GameScene UIGameScene;
    int m_count = 0;

    public void Init()
    {
        UIGameScene = Managers.UI.SceneUI as UI_GameScene;
    }

    public void RefreshUI(UI_Base ui)
    {
        ui.RefreshUI();
    }

    public void RefreshUIAll()
    {
        UIGameScene.UIPlayerInfo.RefreshUI();
        UIGameScene.UIInven.RefreshUI();
        UIGameScene.UISetting.RefreshUI();
        UIGameScene.UIEquipment.RefreshUI();
        UIGameScene.UIShop.RefreshUI();
    }

    public void ShowAndClose(UI_Base scene)
    {
        bool B = scene.gameObject.activeSelf;
        scene.gameObject.SetActive(!B);

        //  UI창을 켰다면
        if (B == false)
        {
            scene.RefreshUI();

            m_count += 1;

            if (m_count == 1)
            {
                // 마우스 커서 Lock Off
                CursorController.MouseCurserLockOnOff(true);
            }
        }
        else
        {
            m_count -= 1;

            if (m_count == 0)
            {
                // 마우스 커서 Lock on
                CursorController.MouseCurserLockOnOff(false);
            }
        }
    }

    List<UI_Base> UIList = new List<UI_Base>();
    public void ShowandCloas<T>(T scene)
    {
        // 만약 처음 키는 거라면
        if (UIList.Contains(scene))
        {
            scene.TryGetComponent
            Managers.UI.ShowPopupUI<scene>;
        }
        // 만약 이미 켜져 있다면
        else
        {

        }
    }
}
