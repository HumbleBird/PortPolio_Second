using System;
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
                PlayAnimation("Idle");
                break;
            case CreatureState.Move:
                MoveAnim();
                break;
            case CreatureState.Skill:
                if(eMoveState == MoveState.Run)
                    PlayAnimation("Run");
                break;
            case CreatureState.Dead:
                PlayAnimation("Dead");
                break;
            default:
                break;
        }
    }

    protected virtual void MoveAnim()
    {
        switch (eMoveState)
        {
            case MoveState.None:
                PlayAnimation("Empty");
                break;
            case MoveState.Walk:
                PlayAnimation("Walk");
                break;
            case MoveState.Run:
                PlayAnimation("Run");
                break;
            case MoveState.Sprint:
                PlayAnimation("Sprint");
                break;
            case MoveState.Falling:
                PlayAnimation("Falling Idle");
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

        PlayAnimation(hitAnimName, true);
    }

    public void PlayAnimation(string animName, bool Interacting = false)
    {
        if (animName == null)
            return;

        m_sCurrentAnimationName = animName;

        if(Interacting)
        {
            float time = Managers.Object.myPlayer.GetAnimationTime(m_sCurrentAnimationName);
            Stop(time);
        }

        m_Animator.CrossFade(animName, m_fNormalizeTransitionDuration);
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
