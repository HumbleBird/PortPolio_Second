using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Stat
{
    public Table_Stat.Info m_tStatInfo { get; set; } = new Table_Stat.Info();

    public int   m_iLevel        { get; set; }
    public int   m_iHp           { get; set; }           
    public int   m_iMaxHp        { get; set; }
    public int   m_iMp           { get; set; }           
    public int   m_iMaxMp        { get; set; }
    public float m_fStemina      { get; set; }
    public float m_fMaxStemina   { get; set; }
    public int   m_iAtk          { get; set; }
    public int   m_fOriginalAtk  { get; set; }
    public int   m_iDef          { get; set; }
    public float m_fMoveSpeed    { get; set; }
    public float m_fWalkSpeed    { get; set; } 
    public float m_fRunSpeed     { get; set; }
    public float m_fAttackSpeed  { get; set; }

    public void Init()
    {
        m_iLevel = 1;
        m_iHp = m_tStatInfo.m_iHp;
        m_iMaxHp = m_iHp;
        m_iMp = m_tStatInfo.m_iMp;
        m_iMaxMp = m_iMp;
        m_fStemina = m_tStatInfo.m_fStemina;
        m_fMaxStemina = m_fStemina;
        m_iAtk = m_tStatInfo.m_iAtk;
        m_fOriginalAtk = m_iAtk;
        m_iDef = m_tStatInfo.m_iDef;
        m_fMoveSpeed = 0;
        m_fWalkSpeed = m_tStatInfo.m_fWalkSpeed;
        m_fRunSpeed = m_tStatInfo.m_fRunSpeed;
        m_fAttackSpeed = m_tStatInfo.m_fAttackSpeed;
    }
}