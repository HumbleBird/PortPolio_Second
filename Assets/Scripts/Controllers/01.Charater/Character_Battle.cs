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
        if (CheckCooltime() == false)
            return;

        m_strAttack.info = Managers.Table.m_Attack.Get(id);

        if (m_strAttack.info == null)
        {
            Debug.LogError($"해당하는 {id}의 스킬이 없습니다.");
            return;
        }

        m_bCanAttack = false;

        // 애니메이션 실행
        ActionAnimation(m_strAttack.info.m_sAnimName);

        // 공격 데미지 더해주기
        m_fCoolTime += m_strAttack.info.m_fCoolTime;
        m_strStat.m_iAtk += m_strAttack.info.m_iDmg;

        // 공격 종료 체크
        StartCoroutine(CoAttackCheck());
    }

    void Attack()
    {
        // 콜라이더 활성화
        AttackCollider tempAttackCollider = AttackCollider.None;

        if (m_strAttack.info.m_nID == m_strAttack.m_iKickNum)
            tempAttackCollider = AttackCollider.CharacterFront;
        else
            tempAttackCollider = AttackCollider.Weapon;

        foreach (var DetectorCollider in m_GoAttackItem)
        {
            if (DetectorCollider.eAttackCollider == tempAttackCollider)
            {
                DetectorCollider.AttackCanOn();
                return;
            }
        }
    }

    // 피격
    public virtual void HitEvent(GameObject attacker, int dmg)
    {
        // 특수 동장으로 인한 데미지 처리
        if (eActionState == ActionState.Invincible)
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
        SetHp(NewHp, attacker);

        // 공격 별 특수 효과
        attacker.GetComponent<Character>().m_strAttack.SpecialAddAttackInfo();

        // 애니메이션
        if(m_strStat.m_iHp > 0)
            HitAnimation();

        float time = GetAnimationTime(m_sCurrentAnimationName);
        Stop(time);
        eState = CreatureState.Idle;
    }

    // 공격 끝
    protected virtual void AttackEnd()
    {
        foreach (var DetectorCollider in m_GoAttackItem)
            DetectorCollider.AttackCanOff();

        m_bCanAttack = true;
        m_bWaiting = false;

        m_strStat.m_iAtk = m_strStat.m_fOriginalAtk;

        eState = CreatureState.Idle;
    }

    public bool CheckCooltime()
    {
        if (m_fCoolTime == 0)
            return true;

        return false;
    }

    protected virtual IEnumerator CoAttackCheck()
    {
        yield return new WaitForSeconds(GetAnimationTime(m_strAttack.info.m_sAnimName, 0.6f));

        if (m_bNextAttack == true && m_strAttack.info.m_iNextNum != 0)
        {
            AttackEvent(m_strAttack.info.m_iNextNum);
            yield break;
        }

        yield return new WaitForSeconds(GetAnimationTime(m_strAttack.info.m_sAnimName, 0.4f));
        {
            AttackEnd();
            yield break;
        }
    }
}
