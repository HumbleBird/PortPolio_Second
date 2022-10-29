using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract partial class Charater : Base
{
    public Table_Stat.Info statInfo { get; set; } = new Table_Stat.Info();
    public Table_Attack.Info attackInfo { get; set; } = new Table_Attack.Info();
    public abstract void SetInfo(int id);

    // 만약
    #region Stat
    public int Hp { get { return statInfo.m_iHp; } set { statInfo.m_iHp = value; } }
    public int MaxHp { get; set; }
    public int Stamina { get { return statInfo.m_iStemina; } set { statInfo.m_iStemina = value; } }
    public int MaxStamina { get; set; }
    public float Atk { get { return statInfo.m_fAtk; } set { statInfo.m_fAtk = value; } }
    public float Def { get { return statInfo.m_fDef; } set { statInfo.m_fDef = value; } }
    public float MoveSpeed { get; set; }
    public float WalkSpeed { get { return statInfo.m_fWalkSpeed; } set { statInfo.m_fWalkSpeed = value; } }
    public float RunSpeed { get { return statInfo.m_fRunSpeed; } set { statInfo.m_fRunSpeed = value; } }
    public float CrounchSpeed { get { return statInfo.m_fCrunchSpeed; } set { statInfo.m_fCrunchSpeed = value; } }
    
    #endregion
}

