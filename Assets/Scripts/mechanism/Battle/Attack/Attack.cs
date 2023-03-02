using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

public class Attack : Strategy
{
    public Table_Attack.Info info;

    public override void Init()
    {
        throw new NotImplementedException();
    }

    public int m_iBasicAttackNum = 1;
    public int m_iStrongAttackNum = 4;
    public int m_iKickNum = 501;

    // 플레이어만 적용
    protected virtual void BasicAttack() 
    {
        // 카메라 쉐이크
        Managers.Camera.m_CameraEffect.Shake(1);
    }

    // 플레이어만 적용
    protected virtual void StrongAttack() { }

    // 넉백 효과
    void Kick()
    {
        Vector3 moveDirection = m_cGo.transform.position - m_cTarget.gameObject.transform.position;
        m_cTarget.Rigid.AddForce(moveDirection.normalized * -100f);
    }

    public void SpecialAddAttackInfo()
    {
        RefreshTargetSet();

        int id = info.m_nID;

        if (id == m_iKickNum)
            Kick();
        else if (id == m_iBasicAttackNum)
            BasicAttack();
        else if (id == m_iStrongAttackNum)
            StrongAttack();
    }

    void RefreshTargetSet()
    {
        m_cTarget = m_cGo.m_goTarget;
    }

    public void Shield()
    {
        if (m_cGo.m_strStat.m_fStemina == 0)
            return;

        if (m_cGo.eMoveState == MoveState.Run)
            m_cGo.SetMoveState(MoveState.Walk);

        m_cGo.eActionState = ActionState.Shield;
    }

    public void Crouch()
    {
        m_cGo.SetMoveState(MoveState.Crouch);
    }

    public void Roll()
    {
        m_cGo.eActionState = ActionState.Invincible;
    }
}
