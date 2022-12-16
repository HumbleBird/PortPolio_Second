using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

public abstract class Strategy
{
    public Dictionary<KeyCode, Action> keyDictionary;
    Dictionary<KeyCode, string> InputKeyDic = new Dictionary<KeyCode, string>();

    protected GameObject m_Go; // 행동 주체자
    protected Character  m_cGo;
    protected GameObject m_GoTarget; // 목표물
    protected Base m_cTarget; // 목표물 스크립트
    protected GameObject m_GoProjectile = null; // 투사체

    protected string m_sActionName = null;

    public virtual void Init(GameObject go)
    {
        m_Go = go.gameObject;
        m_cGo = m_Go.GetComponent<Character>();
    }

    public void InputKey()
    {
        if (Input.anyKeyDown)
        {
            foreach (var dic in keyDictionary)
            {
                if (Input.GetKeyDown(dic.Key))
                {
                    dic.Value();
                    
                    // 입력한 값과 함수 임시 저장
                    InputKeyDic.Add(dic.Key, m_sActionName);
                    ActionStateCheck(m_sActionName, true);
                }
            }
        }

        if (InputKeyDic.Count == 0)
            return;
        
        foreach (var dic in InputKeyDic)
        {
            if (Input.GetKeyUp(dic.Key))
            {
                ActionStateCheck(dic.Value, false);
                InputKeyDic.Remove(dic.Key);
                return;
            }
        }
    }

    public virtual void SetKeyMehod()
    {
        
    }

    public void ActionStateCheck(string actionName, bool bStart)
    {
        if (actionName == null)
            return;

        m_cGo.Animator.SetBool(actionName, bStart);
    }

    public void ActionStateReset()
    {
        foreach (var dic in InputKeyDic)
            ActionStateCheck(dic.Value, false);

        m_sActionName = null;
        m_cGo.eActionState = ActionState.None;
        InputKeyDic.Clear();
    }

    public void ActionStateChange(string actionName)
    {
        switch (actionName)
        {
            case "Shield":
                m_cGo.eActionState = ActionState.Shield;
                break;
            case "Charging":
                m_cGo.eActionState = ActionState.Charging;
                break;
            case "Reload":
                m_cGo.eActionState = ActionState.Reload;
                break;
        }
    }
}
