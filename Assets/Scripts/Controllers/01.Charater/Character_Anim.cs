﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Character : Base
{
	float m_fNormalizeTransitionDuration = 0.15f;
    protected string m_sCurrentAnimationName = null;

    #region Character State Anim
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
                if(eMoveState == MoveState.Run)
                    m_Animator.CrossFade("Run", m_fNormalizeTransitionDuration);
                break;
            case CreatureState.Dead:
                m_Animator.CrossFade("Dead", m_fNormalizeTransitionDuration);
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
                m_Animator.CrossFade("Empty", m_fNormalizeTransitionDuration);
                break;
            case MoveState.Walk:
                m_Animator.CrossFade("Walk", m_fNormalizeTransitionDuration);
                break;
            case MoveState.Run:
                m_Animator.CrossFade("Run", m_fNormalizeTransitionDuration);
                break;
            case MoveState.Falling:
                m_Animator.CrossFade("Falling Idle", m_fNormalizeTransitionDuration);
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

    public float GetAnimationTime(string animName, float time = 1f)
    {
        if (m_DicAniactionclip.ContainsKey(animName) == false)
        {
            Debug.Log("해당 애니메이션 클립이 없습니다." + animName);
            return 0;
        }

        AnimationClip clip = m_DicAniactionclip[animName];
        return clip.length * time;
    }
}
