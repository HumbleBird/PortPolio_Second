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
    protected Charater m_cGo;
    protected GameObject m_GOTarget; // 목표물
    protected GameObject m_GOProjectile = null; // 투사체

    public virtual void Init(GameObject go)
    {
        m_Go = go.gameObject;
        m_cGo = m_Go.GetComponent<Charater>();
    }

    public void InputKey()
    {
        if (Input.anyKeyDown)
        {
            foreach (var dic in keyDictionary)
            {
                if (Input.GetKeyDown(dic.Key))
                    dic.Value();
            }
        }
    }
    public virtual void SetKeyMehod()
    {
        
    }
}
