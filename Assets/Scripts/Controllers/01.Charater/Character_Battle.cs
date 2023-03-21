using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Character : Base
{
    public Attack m_cAttack;

    public Character m_goTarget = null;

    protected bool  m_bCanAttack = true;
    protected bool  m_bNextAttack = false;

    protected Coroutine m_coAttackCheck;

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

        // 공격 데미지 더해주기
        m_strStat.m_iAtk += m_cAttack.m_AttackInfo.m_iDmg;

        // 공격 종료 체크
        m_coAttackCheck = StartCoroutine(CoAttackCheck());
    }

    void Attack()
    {
        // Sound
        SoundPlay(m_cAttack.m_AttackInfo.m_sAnimName);

        // 등록된 이벤트를 실행
        // 근거리라면 애니메이션에 무기 콜라이더를 활성화를 시켜주고
        // 원거리라면 오브젝트를 생성해서 날린다.
        Managers.Battle.ExecuteEventDelegateAttack();
    }

    // 공격 애니메이션을 기점으로 다음 콤보 공격을 할지, 공격을 끝마칠지 결정함.
    IEnumerator CoAttackCheck()
    {
        float time = GetAnimationTime(m_cAttack.m_AttackInfo.m_sAnimName, 0.6f);

        yield return new WaitForSeconds(time);

        if (m_cAttack.m_AttackInfo.m_iNextNum != 0)
        {
            // AI 와 Player를 나눔
            m_bNextAttack = true;
            HowNextAttack();
        }

        time = GetAnimationTime(m_cAttack.m_AttackInfo.m_sAnimName, 0.4f);

        yield return new WaitForSeconds(time);

        AttackEnd();
        yield break;
    }

    protected virtual void HowNextAttack()
    {

    }

    // 피격 판정과 데미지 처리
    public virtual void HitEvent(Character attacker, int dmg)
    {
        // 특수 동작으로 인한 데미지 처리
        if (eActionState == ActionState.Invincible || eState == CreatureState.Dead)
            return;
        else if (eActionState == ActionState.Shield)
        {
            int ShiledHitStamina = 10;
            int shiledHitHpDef = 1;

            float NewStemina = m_strStat.m_fStemina - ShiledHitStamina;
            SetStemina(NewStemina);

            // TODO
            // 나중에 방패 버티기 만큼 감소량 증가 시키기

            m_strStat.m_iDef += shiledHitHpDef;
        }

        // HP 관리
        dmg = Mathf.Max(0, dmg - m_TotalDefence);
        int NewHp = m_strStat.m_iHp - dmg;
        SetHp(NewHp, attacker.gameObject);

        // 등록된 공격 피격 효과 (슬로우, 넉백 등)
        Managers.Battle.ExecuteEventDelegateHitEffect();

        // 애니메이션
        if (m_strStat.m_iHp > 0)
        {
            HitAnimation();

            // 히트 후 Idle로
            float time = GetAnimationTime(m_sCurrentAnimationName);
            Stop(time);
            eState = CreatureState.Idle;
        }
    }

    // 공격 끝
    protected virtual void AttackEnd()
    {
        // 근거리는 트리거 비활성화
        // 원거리는 몰?루
        m_cAttack.AttackEnd();

        m_bCanAttack = true;
        m_bNextAttack = false;

        m_strStat.m_iAtk = m_strStat.m_fOriginalAtk;

        eState = CreatureState.Idle;

        Managers.Battle.ClearEventDelegateAttack();
        Managers.Battle.ClearEventDelegateHitEffect();
    }

    protected void ExcuteNextAttack(int id)
    {
        Managers.Battle.ClearEventDelegateAttack();
        Managers.Battle.ClearEventDelegateHitEffect();
        StopCoroutine(m_coAttackCheck);
        m_bNextAttack = false;
        AttackEvent(id);
    }
}
