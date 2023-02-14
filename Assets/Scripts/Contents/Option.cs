using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

public partial class Option
{
    Dictionary<KeyCode, Action> keyDictionary; // 단발성
    
    public void SetKey()
    {
        keyDictionary = new Dictionary<KeyCode, Action>
        {
            { Managers.InputKey._binding.Bindings[UserAction.UI_Setting], ShowInputKeySetting},
            { Managers.InputKey._binding.Bindings[UserAction.UI_Inventory], ShowInventory},
            { Managers.InputKey._binding.Bindings[UserAction.UI_Equipment], ShowEquipment},
        };
    }

    public void InputOptionKey()
    {
        if(Input.anyKeyDown)
        {
            foreach (var dic in keyDictionary)
            {
                if(Input.GetKeyDown(dic.Key))
                {
                    dic.Value();
                }
            }
        }
    }

    void ShowInputKeySetting()
    {
        ShowAndClose(Managers.UIBattle.UIGameScene.UISetting);
    }

    void ShowInventory()
    {
        Managers.UIBattle.InvenRefreshUI();
        ShowAndClose(Managers.UIBattle.UIGameScene.UIInven);
    }

    void ShowEquipment()
    {
        ShowAndClose(Managers.UIBattle.UIGameScene.UIEquipment);
    }

    void ShowAndClose(UI_Base scene)
    {
        bool B = scene.gameObject.activeSelf;
        scene.gameObject.SetActive(!B);

        // UI On
        //if(B == false)
        //{
        //    Cursor.lockState = CursorLockMode.None;
        //    Managers.Camera2.CemeraPauseOnUI();
        //}
        //// UI Off
        //else
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //    Managers.Camera2.CemeraPauseOnUI();
        //}
    }
}
