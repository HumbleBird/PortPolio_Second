using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

public partial class Option
{


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
    }
}
