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

    protected GameObject m_Go; // 행동 주체자
    protected Character m_cGo;
    protected GameObject m_GoTarget; // 목표물
    protected GameObject m_GoProjectile = null; // 투사체

    protected string m_sActionName;

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
                if (Input.GetKey(dic.Key))
                {
                    dic.Value();
                    ActionState(m_sActionName, true);
                }
                else if (Input.GetKeyUp(dic.Key))
                {
                    ActionState(m_sActionName, false);
                }
            }
        }
    }

    public virtual void SetKeyMehod()
    {
        
    }

    public void ActionState(string action, bool IsStart)
    {
        m_cGo.Animator.SetBool($"{action}", IsStart);
    }
}
