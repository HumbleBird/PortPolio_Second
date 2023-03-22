using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

public partial class MyPlayer : Player
{
    Dictionary<KeyCode, Action> OptionKeyDic; // ´Ü¹ß¼º

    public void SetKey()
    {
        OptionKeyDic = new Dictionary<KeyCode, Action>
        {
            { Managers.InputKey._binding.Bindings[UserAction.UI_Setting], () => {    Managers.UIBattle.ShowAndCloseUI<UI_SettingKey>(); }},
            { Managers.InputKey._binding.Bindings[UserAction.UI_Inventory], () => {  Managers.UIBattle.ShowAndCloseUI<UI_Inven>(); }},
            { Managers.InputKey._binding.Bindings[UserAction.UI_Equipment], () => {  Managers.UIBattle.ShowAndCloseUI<UI_Equipment>(); }},
        };
    }

    public void InputOptionKey()
    {
        if (Input.anyKeyDown)
        {
            foreach (var dic in OptionKeyDic)
            {
                if (Input.GetKeyDown(dic.Key))
                {
                    dic.Value();

                    // Sound
                    SoundPlay("UI On Off Sound");
                }
            }
        }
    }
}
