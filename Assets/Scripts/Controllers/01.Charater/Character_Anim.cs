using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Character : Base
{
	float m_fNormalizeTransitionDuration = 0.15f;
    string m_sCurrentAnimationName = null;

	public virtual void UpdateAnimation()
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

    #region CharacterStandAnim
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

        m_sCurrentAnimationName = hitAnimName;

        Animator.CrossFade(hitAnimName, m_fNormalizeTransitionDuration);
    }

    // 공격 및 특수 액션 애니메이션
    public string ActionAnimation(string animName, bool bStart = true)
    {
        if (animName == null)
            return null;

        AnimationLayers animLayer = SetLayer(animName);

        // 상태에 따른 애니메이션 이름 결정
         if (animName == UserAction.Roll.ToString())
        {
            if (eState == CreatureState.Idle)
                animName = "Stand To Roll";
            else if (eState == CreatureState.Move)
                animName = "Run To Roll";
        }

        if (bStart == false)
            animName = animName + " End";

        Animator.CrossFade(animName, m_fNormalizeTransitionDuration, (int)animLayer);

        return animName;
    }

    private AnimationLayers SetLayer(string animName)
    {
        // TODO
        AnimationLayers animLayer = AnimationLayers.BaseLayer;

        if (animName == "Shield")
            animLayer = AnimationLayers.UpperLayer;

        return animLayer;
    }

}
