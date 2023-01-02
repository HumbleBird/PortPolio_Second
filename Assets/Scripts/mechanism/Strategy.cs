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

    public virtual void Init(GameObject go)
    {
        m_Go = go.gameObject;
        m_cGo = m_Go.GetComponent<Character>();
    }

    public virtual void SetKeyMehod() { }

    // 연속성 (쉴드, 앉기, 장전 등)
    public void InputMaintainKey()
    {
        if (Input.anyKeyDown)
        {
            foreach (var dic in MaintainkeyDictionary)
            {
                if (Input.GetKey(dic.Key))
                {
                    dic.Value();

                    if (InputKeyDic.ContainsKey(dic.Key))
                        return;
                    
                    // 입력한 값과 함수 임시 저장
                    InputKeyDic.Add(dic.Key, m_sAnimationName);
                    m_cGo.StrAnimation(m_sAnimationName);
                }
            }
        }

        if (InputKeyDic.Count == 0)
            return;
        
        foreach (var dic in InputKeyDic)
        {
            if (Input.GetKeyUp(dic.Key))
            {
                m_cGo.StrAnimation(dic.Value, false);
                ActionStateReset();
                InputKeyDic.Remove(dic.Key);
                return;
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
                    dic.Value();
                    m_cGo.StrAnimation(m_sAnimationName);
                }
            }
        }
    }

    public void ActionStateReset()
    {
        InputKeyDic.Clear();
        m_cGo.m_bWaiting = false;

        m_cGo.eActionState = ActionState.None;
        m_sAnimationName = null;
    }

    public void ActionStateChange(string actionName)
    {
        foreach (ActionState state in Enum.GetValues(typeof(ActionState)))
        {
            if(actionName == state.ToString())
                m_cGo.eActionState = state;
        }

        if (actionName == "Crouch")
            m_cGo.m_bWaiting = false;
    }
}
