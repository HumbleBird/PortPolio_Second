using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract partial class Character : Base
{
    public Table_Stat.Info m_tStatInfo { get; set; } = new Table_Stat.Info();
    public Table_Attack.Info m_tAttackInfo { get; set; } = new Table_Attack.Info();
    
    protected Strategy m_strCharacterAction = new PlayerAction();

    public ActionState eActionState = ActionState.None;

    public abstract void SetInfo(int id);

    #region Stat
    public int Hp { get { return m_tStatInfo.m_iHp; } set { m_tStatInfo.m_iHp = value; } }
    public int MaxHp { get; set; }
    public int Stamina { get { return m_tStatInfo.m_iStemina; } set { m_tStatInfo.m_iStemina = value; } }
    public int MaxStamina { get; set; }
    public float Atk { get { return m_tStatInfo.m_fAtk; } set { m_tStatInfo.m_fAtk = value; } }
    public float Def { get { return m_tStatInfo.m_fDef; } set { m_tStatInfo.m_fDef = value; } }
    public float MoveSpeed { get; set; }
    public float WalkSpeed { get { return m_tStatInfo.m_fWalkSpeed; } set { m_tStatInfo.m_fWalkSpeed = value; } }
    public float RunSpeed { get { return m_tStatInfo.m_fRunSpeed; } set { m_tStatInfo.m_fRunSpeed = value; } }
    public float CrounchSpeed { get { return m_tStatInfo.m_fCrunchSpeed; } set { m_tStatInfo.m_fCrunchSpeed = value; } }
    #endregion
}

