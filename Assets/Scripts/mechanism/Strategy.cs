using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

public abstract class Strategy
{
    public Dictionary<KeyCode, Action> MaintainkeyDictionary; // 연속성
    public Dictionary<KeyCode, Action> OnekeyDictionary; // 단발성
    public Dictionary<KeyCode, string> InputKeyDic = new Dictionary<KeyCode, string>(); // 키 값 저장

    protected GameObject m_Go; // 행동 주체자
    protected Character  m_cGo;
    protected GameObject m_GoTarget; // 목표물
    protected Base m_cTarget; // 목표물 스크립트
    protected GameObject m_GoProjectile = null; // 투사체

    public string m_sAnimationName = null;
    public Layers m_eAnimLayers = Layers.BaseLayer;

    public void SetInfo(GameObject go)
    {
        m_Go = go.gameObject;
        m_cGo = m_Go.GetComponent<Character>();
    }

    protected void PlayAnimation(string AnimName, bool bStart)
    {
        m_eAnimLayers = Layers.BaseLayer;

        if(AnimName == "Shield")
        {
            m_eAnimLayers = Layers.UpperLayer;
        }

        m_cGo.StrAnimation(AnimName, bStart, m_eAnimLayers);
    }

    // 연속성 (쉴드, 앉기, 장전 등)
    public void InputMaintainKey()
    {
        if (Input.anyKey)
        {
            foreach (var dic in MaintainkeyDictionary)
            {
                if (Input.GetKey(dic.Key))
                {
                    if (!InputKeyDic.ContainsKey(dic.Key))
                    {
                        dic.Value();
                        // 입력한 값과 함수 임시 저장
                        InputKeyDic.Add(dic.Key, m_sAnimationName);
                        PlayAnimation(m_sAnimationName, true);
                    }
                }
            }
        }

        if (InputKeyDic.Count == 0)
            return;

        var RemoveKey = InputKeyDic.ToArray();

        foreach (var dic in RemoveKey)
        {
            if (Input.GetKeyUp(dic.Key))
            {
                PlayAnimation(dic.Value, false);
                InputKeyDic.Remove(dic.Key);
            }
        }
    }

    // 단발성(점프, 구르기 등)
    public virtual void InputOnekey()
    {
        if (Input.anyKeyDown)
        {
            foreach (var dic in OnekeyDictionary)
            {
                if (Input.GetKeyDown(dic.Key))
                {
                    if (!InputKeyDic.ContainsKey(dic.Key))
                    {
                        InputKeyDic.Add(dic.Key, m_sAnimationName);

                        m_cGo.m_bWaiting = true;
                        dic.Value();
                        m_cGo.StrAnimation(m_sAnimationName);
                    }
                }
            }
        }
    }

    public void ActionStateReset(string ActionKinds = "")
    {
        if (ActionKinds == "")
        {
            InputKeyDic.Clear();
        }
        else if(ActionKinds == "eAction")
        {
            InputKeyDic.Remove(Managers.InputKey._binding.Bindings[UserAction.Shield]);
        }
        else if(ActionKinds == "eMove")
        {
            m_cGo.eMoveState = MoveState.None;
        }

        m_cGo.eActionState = ActionState.None;
        m_cGo.m_bWaiting = false;
        m_sAnimationName = null;
    }

    public void StateAllClear()
    {

    }


    public void ActionStateChange(string actionName)
    {
        if (actionName == "Crouch")
        {
            m_cGo.m_bWaiting = false;
        }

        // 생각보다 시간을 꽤 잡아먹은 0.5초?
        foreach (ActionState state in Enum.GetValues(typeof(ActionState)))
        {
            if(actionName == state.ToString())
                m_cGo.eActionState = state;
        }
    }
}
