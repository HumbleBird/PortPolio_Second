﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

public abstract class Strategy
{
    public Dictionary<KeyCode, Action> keyDictionary;

    protected GameObject m_Go; // 행동 주체자
    protected Character  m_cGo;
    protected GameObject m_GoTarget; // 목표물
    protected GameObject m_GoProjectile = null; // 투사체

    protected string ActionName = null;

    public virtual void Init(GameObject go)
    {
        m_Go = go.gameObject;
        m_cGo = m_Go.GetComponent<Character>();
    }

    bool holdingDown;
    public void InputKey()
    {
        if (Input.anyKeyDown)
        {
            foreach (var dic in keyDictionary)
            {
                if (Input.GetKeyDown(dic.Key))
                {
                    dic.Value();
                    if(ActionName != null)
                        ActionStateCheck(ActionName, true);
                }
            }
            holdingDown = true;
        }
        else if (!Input.anyKey && holdingDown)
        {
            foreach (var dic in keyDictionary)
            {
                if (Input.GetKeyUp(dic.Key))
                {
                    ActionStateReset();
                }
            }

            holdingDown = false;
        }
    }

    public virtual void SetKeyMehod()
    {
        
    }

    public void ActionStateCheck(string action, bool IsStart)
    {
        m_cGo.Animator.SetBool($"{action}", IsStart);
    }

    public void ActionStateReset()
    {
        ActionStateCheck(ActionName, false);
        if(ActionName != null)
            ActionName = null;
        m_cGo.eActionState = ActionState.None;
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
