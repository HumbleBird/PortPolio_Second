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

        //  UIâ�� �״ٸ�
        if (B == false)
        {
            scene.RefreshUI();

            m_count += 1;

            if (m_count == 1)
            {
                // ���콺 Ŀ�� Lock Off
                CursorController.MouseCurserLockOnOff(true);
            }
        }
        else
        {
            m_count -= 1;

            if (m_count == 0)
            {
                // ���콺 Ŀ�� Lock on
                CursorController.MouseCurserLockOnOff(false);
            }
        }
    }

    List<UI_Base> UIList = new List<UI_Base>();
    public void ShowandCloas<T>(T scene)
    {
        // ���� ó�� Ű�� �Ŷ��
        if (UIList.Contains(scene))
        {
            scene.TryGetComponent
            Managers.UI.ShowPopupUI<scene>;
        }
        // ���� �̹� ���� �ִٸ�
        else
        {

        }
    }
}
