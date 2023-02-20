using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

public abstract class Strategy
{
    // 오브젝트
    protected Character  m_cGo; // 공격자
    protected Character m_cTarget;  // 피격자
    protected GameObject m_GoProjectile = null; // 투사체

    public void SetInfo(Character character)
    {
        m_cGo = character;
    }

    public abstract void Init();
}
