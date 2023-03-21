using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Character : Base
{
	float m_fNormalizeTransitionDuration = 0.15f;
    protected string m_sCurrentAnimationName = null;

	public virtual void UpdateAnimation()
    {
        switch (eState)
        {
            case CreatureState.Idle:
                m_Animator.CrossFade("Idle", m_fNormalizeTransitionDuration);
                break;
            case CreatureState.Move:
                MoveAnim();
                break;
            case CreatureState.Skill:
                break;
            case CreatureState.Dead:
                m_Animator.CrossFade("Dead", m_fNormalizeTransitionDuration);
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
                m_Animator.CrossFade("Idle", m_fNormalizeTransitionDuration);
                break;
            case MoveState.Walk:
                m_Animator.CrossFade("Walk", m_fNormalizeTransitionDuration);
                break;
            case MoveState.Run:
                m_Animator.CrossFade("Run", m_fNormalizeTransitionDuration);
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

        m_sCurrentAnimationName = hitAnimName;

        m_Animator.CrossFade(hitAnimName, m_fNormalizeTransitionDuration);
    }


    // 공격 및 특수 액션 애니메이션
    public void PlayAnimation(string animName)
    {
        if (animName == null)
            return;

        AnimationLayers layer = SetLayer(animName);

        m_Animator.CrossFade(animName, m_fNormalizeTransitionDuration, (int)layer);
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
