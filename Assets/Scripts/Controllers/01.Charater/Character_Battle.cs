using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract  partial class Character : Base
{
    public Attack m_cAttack;
    public Character m_goTarget = null;
    protected bool  m_bCanAttack = true;

    public virtual float m_TotalAttack { get { return m_Stat.m_iAtk; } }
    public virtual float m_TotalDefence { get { return m_Stat.m_iDef; } }

    public virtual void AttackEvent(int id)
    {
        m_cAttack.m_AttackInfo = Managers.Table.m_Attack.Get(id);

        if (m_cAttack.m_AttackInfo == null)
        {
            Debug.LogError($"해당하는 {id}의 스킬이 없습니다.");
            return;
        }

        m_bCanAttack = false;

        // 애니메이션 실행
        PlayAnimation(m_cAttack.m_AttackInfo.m_sName);

        // 사운드
        SoundPlay(m_cAttack.m_AttackInfo.m_sName);

        // 어택 이벤트 추가 //Blow면 무기 트리거 키기, Range면 화살 발사
        //Managers.Battle.EventDelegateAttack += m_cAttack.NormalAttack;
    }

    void Attack()
    {
        // 배틀 매니저에 등록된 이벤트를 실행
        Managers.Battle.ExecuteEventDelegateAttack();
    }

    // 피격 판정과 데미지 처리
    public virtual void HitEvent(Character attacker, float dmg, bool isAnimation = true)
    {
        // 특수 동작으로 인한 데미지 처리
        if (eActionState == ActionState.Invincible || eState == CreatureState.Dead)
            return;

        // 데미지 공식 = (무기 데미지 - 적 방어력) / 피해 감소율
        dmg = Mathf.Max(0, dmg - m_TotalDefence); 
        int NewHp = (int)(m_Stat.m_iHp - dmg);
        SetHp(NewHp);

        // 등록된 공격 피격 효과 (슬로우, 넉백 등)
        Managers.Battle.ExecuteEventDelegateHitEffect();

        // 애니메이션
        if (m_Stat.m_iHp > 0)
        {
            if(isAnimation)
            {
                HitAnimation();

                // 히트 후 Idle로
                float time = GetAnimationTime(m_sCurrentAnimationName);
                WaitToState(time, CreatureState.Idle);
            }
        }
    }

    // 공격 끝
    protected virtual void AttackEnd()
    {
        Managers.Battle.ExecuteEventDelegateAttackEnd(); // Blow의 경우 무기 콜라이더 꺼주기
        Managers.Battle.ClearAllEvnetDelegate();
    }
}
