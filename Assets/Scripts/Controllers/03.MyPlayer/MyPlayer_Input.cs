using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

public partial class MyPlayer : Player
{
    public void SetKey()
    {
        MaintainkeyDictionary = new Dictionary<KeyCode, Action>
        {
            { Managers.InputKey._binding.Bindings[UserAction.Crouch], m_strAttack.Crouch},
            { Managers.InputKey._binding.Bindings[UserAction.Shield], m_strAttack.Shield},
        };

        OnekeyDictionary = new Dictionary<KeyCode, Action>
        {
            // 액션
            //Managers.InputKey._binding.Bindings[UserAction.Jump], Jump},
            { Managers.InputKey._binding.Bindings[UserAction.Roll], m_strAttack.Roll},
        };

        OptionKeyDic = new Dictionary<KeyCode, Action>
        {
            { Managers.InputKey._binding.Bindings[UserAction.UI_Setting], m_cOption.ShowInputKeySetting},
            { Managers.InputKey._binding.Bindings[UserAction.UI_Inventory], m_cOption.ShowInventory},
            { Managers.InputKey._binding.Bindings[UserAction.UI_Equipment], m_cOption.ShowEquipment},
        };
    }

    #region Action

    protected Dictionary<KeyCode, Action> MaintainkeyDictionary; // 연속성
    protected Dictionary<KeyCode, Action> OnekeyDictionary; // 단발성

    // 연속성 (쉴드, 앉기, 장전 등)
    public void InputMaintainKey()
    {
        Dictionary<KeyCode, Action> InputKeyDic = new Dictionary<KeyCode, Action>(); // 키 값 저장

        // 키를 누르고 있을 때
        if (Input.anyKey)
        {
            foreach (var dic in MaintainkeyDictionary)
            {
                if (Input.GetKey(dic.Key))
                {
                    if (!InputKeyDic.ContainsKey(dic.Key))
                    {
                        SpeicialAction(dic.Value);

                        // 입력한 값과 함수 임시 저장
                        InputKeyDic.Add(dic.Key, dic.Value);
                    }
                }
            }
        }

        if (InputKeyDic.Count == 0)
            return;

        var removeDic = InputKeyDic.ToArray();

        // 키를 올렸을 때
        foreach (var dic in removeDic)
        {
            if (Input.GetKeyUp(dic.Key))
            {
                SpeicialAction(dic.Value, false);

                // 초기화
                if (dic.Key == Managers.InputKey._binding.Bindings[UserAction.Crouch])
                {
                    SetMoveState(MoveState.Walk);
                }

                if (dic.Key == Managers.InputKey._binding.Bindings[UserAction.Shield])
                {
                    eMoveState = MoveState.None;
                }

                InputKeyDic.Remove(dic.Key);
            }
        }
    }

    // 단발성(점프, 구르기, 공격도 가능할 듯?)
    public virtual void InputOnekey()
    {
        Dictionary<KeyCode, Action> InputKeyDic = new Dictionary<KeyCode, Action>(); // 키 값 저장

        if (Input.anyKeyDown)
        {
            foreach (var dic in OnekeyDictionary)
            {
                if (Input.GetKeyDown(dic.Key))
                {
                    if (!InputKeyDic.ContainsKey(dic.Key))
                    {
                        SpeicialAction(dic.Value);

                        InputKeyDic.Add(dic.Key, dic.Value);
                    }
                }
            }
        }
    }

    #endregion


    #region Option

    Dictionary<KeyCode, Action> OptionKeyDic; // 단발성

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

    #endregion

}
