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

    public void ChangeClass(int ClassId)
    {
        eCharacterClass = (CharacterClass)ClassId;

        switch (eCharacterClass)
        {
            case CharacterClass.None:
                break;
            case CharacterClass.Warior:
                m_cAttack = new Warior();
                break;
            case CharacterClass.Knight:
                m_cAttack = new Knight();
                break;
            case CharacterClass.Archer:
                m_cAttack = new Archer();
                break;
            case CharacterClass.Wizard:
                m_cAttack = new Wizard();
                break;
            default:
                break;
        }

        m_cAttack.SetInfo(this);
    }

    protected virtual void AttackEvent(int id)
    {
        // TODO 공격 쿨타임 체크

        m_cAttack.m_AttackInfo = Managers.Table.m_Attack.Get(id);

        if (m_cAttack.m_AttackInfo == null)
        {
            Debug.LogError($"해당하는 {id}의 스킬이 없습니다.");
            return;
        }

        m_bCanAttack = false;

        // 애니메이션 실행
        PlayAnimation(m_cAttack.m_AttackInfo.m_sAnimName);

        // 사운드
        SoundPlay(m_cAttack.m_AttackInfo.m_sAnimName);

        // 공격 데미지 더해주기
        m_Stat.m_iAtk += m_cAttack.m_AttackInfo.m_iDmg;
    }

    void Attack()
    {
        // 배틀 매니저에 등록된 이벤트를 실행
        Managers.Battle.ExecuteEventDelegateAttack();
    }

    // 공격 애니메이션의 길이를 체크해서 다음 콤보 공격의 허용과 공격 끝을 알림.
    protected abstract IEnumerator CoAttackCheck();

    // 피격 판정과 데미지 처리
    public virtual void HitEvent(Character attacker, int dmg)
    {
        HitEventBaseOnState();

        // HP 관리
        dmg = Mathf.Max(0, dmg - m_TotalDefence);
        int NewHp = m_Stat.m_iHp - dmg;
        SetHp(NewHp, attacker.gameObject);

        // 등록된 공격 피격 효과 (슬로우, 넉백 등)
        Managers.Battle.ExecuteEventDelegateHitEffect();

        // 애니메이션
        if (m_Stat.m_iHp > 0)
        {
            HitAnimation();

            // 히트 후 Idle로
            float time = GetAnimationTime(m_sCurrentAnimationName);
            Stop(time);
            eState = CreatureState.Idle;
        }
    }

    void HitEventBaseOnState()
    {
        // 특수 동작으로 인한 데미지 처리
        if (eActionState == ActionState.Invincible || eState == CreatureState.Dead)
            return;
        else if (eActionState == ActionState.Shield)
        {
            int ShiledHitStamina = 10;
            int shiledHitHpDef = 1;

            float NewStemina = m_Stat.m_fStemina - ShiledHitStamina;
            SetStemina(NewStemina);

            // TODO
            // 나중에 방패 버티기 만큼 감소량 증가 시키기

            m_Stat.m_iDef += shiledHitHpDef;
        }
    }

    // 공격 끝
    protected virtual void AttackEnd()
    {
        m_Stat.m_iAtk = m_Stat.m_fOriginalAtk;

        Managers.Battle.ExecuteEventDelegateAttackEnd(); // Blow의 경우 무기 콜라이더 꺼주기
        Managers.Battle.ClearAllEvnetDelegate();
    }
}
