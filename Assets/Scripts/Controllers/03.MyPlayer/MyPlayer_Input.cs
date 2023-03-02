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
            // �׼�
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

    protected Dictionary<KeyCode, Action> MaintainkeyDictionary; // ���Ӽ�
    protected Dictionary<KeyCode, Action> OnekeyDictionary; // �ܹ߼�

    // ���Ӽ� (����, �ɱ�, ���� ��)
    public void InputMaintainKey()
    {
        Dictionary<KeyCode, Action> InputKeyDic = new Dictionary<KeyCode, Action>(); // Ű �� ����

        // Ű�� ������ ���� ��
        if (Input.anyKey)
        {
            foreach (var dic in MaintainkeyDictionary)
            {
                if (Input.GetKey(dic.Key))
                {
                    if (!InputKeyDic.ContainsKey(dic.Key))
                    {
                        SpeicialAction(dic.Value);

                        // �Է��� ���� �Լ� �ӽ� ����
                        InputKeyDic.Add(dic.Key, dic.Value);
                    }
                }
            }
        }

        if (InputKeyDic.Count == 0)
            return;

        var removeDic = InputKeyDic.ToArray();

        // Ű�� �÷��� ��
        foreach (var dic in removeDic)
        {
            if (Input.GetKeyUp(dic.Key))
            {
                SpeicialAction(dic.Value, false);

                // �ʱ�ȭ
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

    // �ܹ߼�(����, ������, ���ݵ� ������ ��?)
    public virtual void InputOnekey()
    {
        Dictionary<KeyCode, Action> InputKeyDic = new Dictionary<KeyCode, Action>(); // Ű �� ����

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

    Dictionary<KeyCode, Action> OptionKeyDic; // �ܹ߼�

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
