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
        //ShowAndClose(Managers.UIBattle.UISetting);
    }

    void ShowInventory()
    {
        //ShowAndClose(Managers.UIBattle.UIInven);
    }

    void ShowAndClose(UI_Scene popup)
    {
        bool B = popup.gameObject.activeSelf;
        popup.gameObject.SetActive(!B);
    }
}
