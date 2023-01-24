using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Stat
{
    public Table_Stat.Info m_tStatInfo { get; set; } = new Table_Stat.Info();

    public int   m_iLevel { get; set; }
    public float m_fHp               { get; set; }           
    public float m_fMaxHp        { get; set; }
    public float m_fMp               { get; set; }           
    public float m_fMaxMp        { get; set; }
    public float m_fStemina       { get; set; }
    public float m_fMaxStemina      { get; set; }
    public float m_fAtk         { get; set; }
    public float m_fOriginalAtk         { get; set; }
    public float m_fDef         { get; set; }
    public float m_fJumpPower       { get; set; }
    public float m_fMoveSpeed { get; set; }
    public float m_fWalkSpeed       { get; set; } 
    public float m_fRunSpeed        { get; set; }
    public float m_fCrouchSpeed  { get; set; }
    public float m_fAttackSpeed     { get; set; }

    public void Init()
    {
        m_iLevel = 1;
        m_fHp = m_tStatInfo.m_fHp;
        m_fMaxHp = m_fHp;
        m_fMp = 100; //m_tStatInfo.m_fHp;
        m_fMaxMp = 100;// m_fHp;
        m_fStemina = m_tStatInfo.m_fStemina;
        m_fMaxStemina = m_fStemina;
        m_fAtk = m_tStatInfo.m_fAtk;
        m_fOriginalAtk = m_fAtk;
        m_fDef = m_tStatInfo.m_fDef;
        m_fJumpPower = m_tStatInfo.m_fJumpPower;
        m_fMoveSpeed = 0;
        m_fWalkSpeed = m_tStatInfo.m_fWalkSpeed;
        m_fRunSpeed = m_tStatInfo.m_fRunSpeed;
        m_fCrouchSpeed = m_tStatInfo.m_fCrouchSpeed;
        m_fAttackSpeed = m_tStatInfo.m_fAttackSpeed;
    }
}