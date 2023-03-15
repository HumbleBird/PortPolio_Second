using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Character : Base
{
    public Attack m_strAttack;
    public Character m_goTarget { get; set; } // 타겟

    protected float m_fCoolTime = 0f;
    protected bool  m_bCanAttack = true;
    protected bool  m_bNextAttack = false;

    protected Coroutine m_coAttackCheck;

    public void ChangeClass(string typeClass)
    {
        switch (typeClass)
        {
            case "Blow":
                m_strAttack = new Blow();
                break;
            case "Range":
                m_strAttack = new Range();
                break;
            default:
                break;
        }
    }

    protected virtual void AttackEvent(int id)
    {
        // TODO 공격 쿨타임 체크

        m_strAttack.info = Managers.Table.m_Attack.Get(id);

        if (m_strAttack.info == null)
        {
            Debug.LogError($"해당하는 {id}의 스킬이 없습니다.");
            return;
        }

        eState = CreatureState.Skill;

        m_bCanAttack = false;
        m_bWaiting = true;
        m_bNextAttack = false;

        // 애니메이션 실행
        ActionAnimation(m_strAttack.info.m_sAnimName);

        // 공격 데미지 더해주기
        //m_fCoolTime += m_strAttack.info.m_fCoolTime;
        m_strStat.m_iAtk += m_strAttack.info.m_iDmg;

        // 공격 종료 체크
        m_coAttackCheck = StartCoroutine(CoAttackCheck());
    }

    void Attack()
    {
        // Sound
        SoundPlay(m_strAttack.info.m_sAnimName);

        // 이건 battle manager에서 delegate로 하기

        // 근거리
        // 애니메이션 자체에서 함수를 실행 -> 무기 콜라이더 활성화

        // 원거리
        // 공격 준비 중, 공격 시작을 공격자에서 알림 -> 오브젝트 소환
    }

    // 피격
    public virtual void HitEvent(Character attacker, int dmg)
    {
        // 특수 동장으로 인한 데미지 처리
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

        // TODO 공격 별 특수 효과
        attacker.m_strAttack.SpecialAddAttackInfo();

        // 애니메이션
        if(m_strStat.m_iHp > 0)
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
        foreach (var DetectorCollider in m_GoAttackItem)
            DetectorCollider.AttackCanOff();

        m_bCanAttack = true;
        m_bWaiting = false;
        m_bNextAttack = false;

        m_strStat.m_iAtk = m_strStat.m_fOriginalAtk;

        eState = CreatureState.Idle;

        Managers.Battle.ClearEventDelegateAttack();
    }

    public bool CheckCooltime()
    {
        if (m_fCoolTime == 0)
            return true;

        return false;
    }

    IEnumerator CoAttackCheck()
    {
        float time = GetAnimationTime(m_strAttack.info.m_sAnimName, 0.6f);

        yield return new WaitForSeconds(time);

        if (m_strAttack.info.m_iNextNum != 0)
        {
            HowNextAttack();
        }

        time = GetAnimationTime(m_strAttack.info.m_sAnimName, 0.4f);

        yield return new WaitForSeconds(time);

        AttackEnd();
        yield break;
    }

    protected virtual void HowNextAttack()
    {

    }
}
