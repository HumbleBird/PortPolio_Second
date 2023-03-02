using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

public class Option
{
    int m_count = 0;

    public void ShowInputKeySetting()
    {
        ShowAndClose(Managers.UIBattle.UIGameScene.UISetting);
    }

    public void ShowInventory()
    {
        Managers.UIBattle.InvenRefreshUI();
        ShowAndClose(Managers.UIBattle.UIGameScene.UIInven);
    }

    public void ShowEquipment()
    {
        Managers.UIBattle.EquipmentRefreshUI();
        ShowAndClose(Managers.UIBattle.UIGameScene.UIEquipment);
    }

    void ShowAndClose(UI_Base scene)
    {
        bool B = scene.gameObject.activeSelf;
        scene.gameObject.SetActive(!B);

        //  UI창을 켰다면
        if(B == false)
        {
            m_count += 1;

            if(m_count == 1)
            {
                // 마우스 커서 Lock Off
                CursorController.MouseCurserLockOnOff(true);
            }
        }
        else
        {
            m_count -= 1;

            if(m_count == 0)
            {
                // 마우스 커서 Lock on
                CursorController.MouseCurserLockOnOff(false);
            }
        }
    }
}
