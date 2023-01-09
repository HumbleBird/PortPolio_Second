using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Character : Base
{
	public float m_fNormalizeTransitionDuration = 0.14f;

	public virtual void UpdateAnimation()
    {
        CharacterStateAnimation();
    }

    #region CharacterStandAnim
    void CharacterStateAnimation()
    {
        switch (eState)
        {
            case CreatureState.Idle:
                Animator.CrossFade("Idle", m_fNormalizeTransitionDuration);
                break;
            case CreatureState.Move:
                MoveAnim();
                break;
            case CreatureState.Skill:
                break;
            case CreatureState.Dead:
                Animator.CrossFade("Dead", m_fNormalizeTransitionDuration);
                break;
            default:
                break;
        }
    }

    void MoveAnim()
    {
        switch (eMoveState)
        {
            case MoveState.None:
                Animator.CrossFade("Idle", m_fNormalizeTransitionDuration);
                break;
            case MoveState.Walk:
                Animator.CrossFade("Walk", m_fNormalizeTransitionDuration);
                break;
            case MoveState.Run:
                Animator.CrossFade("Run", m_fNormalizeTransitionDuration);
                break;
            case MoveState.Crouch:
                Animator.CrossFade("Crouch Walk Forward", m_fNormalizeTransitionDuration);
                break;
            default:
                break;
        }
    }
    #endregion

    protected void HitAnimation()
    {
        string hitAnimName = HitMotion.NormalHit.ToString();

        if (eActionState == ActionState.Shield)
            hitAnimName = HitMotion.ShieldHit.ToString();
        else if (eMoveState == MoveState.Crouch && eActionState == ActionState.Shield)
            hitAnimName = HitMotion.CrouchShieldHit.ToString();
        else if (eMoveState == MoveState.Crouch)
            hitAnimName = HitMotion.CrouchingHit.ToString();

        Animator.CrossFade(hitAnimName, m_fNormalizeTransitionDuration);
    }

    // 공격 및 특수 액션 애니메이션
    public void StrAnimation(string animName, bool bStart = true, Layers animLayer = Layers.BaseLayer)
    {
        if (animName == null)
            return;

        if (bStart)
            Animator.CrossFade(animName, m_fNormalizeTransitionDuration, (int)animLayer);
        else
            Animator.CrossFade(animName + " End", m_fNormalizeTransitionDuration, (int)animLayer);
    }
}
