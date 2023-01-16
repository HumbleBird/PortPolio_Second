﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Character : Base
{
    public Attack m_strAttack;
    public GameObject m_goTarget { get; set; } // 타겟

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
        eState = CreatureState.Skill;

        // 애니메이션 실행
        StrAnimation(m_strAttack.info.m_sAnimName);

        // 공격 데미지 더해주기
        m_fCoolTime += m_strAttack.info.m_fCoolTime;
        m_strStat.m_fAtk += m_strAttack.info.m_fDmg;

        // 스테미너 감소
        m_strStat.m_fStemina -= 10f;
        SetStemina(m_strStat.m_fStemina);
    }

    void Attack()
    {
        m_goTarget = null;

        // 콜라이더 활성화
        AttackCollider tempAttackCollider = AttackCollider.None;

        if (m_strAttack.info.m_nID == m_strAttack.m_iKickNum || gameObject.tag == "Monster") // TODO
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
    public virtual void HitEvent(GameObject attacker, float dmg)
    {
        if (eActionState == ActionState.Invincible)
            return;
        else if (eActionState == ActionState.Shield)
        {
            int ShiledHitStamina = 10;
            float shiledHitHpDef = 1.0f;

            float NewStemina = m_strStat.m_fStemina - ShiledHitStamina;
            SetStemina(NewStemina);
            dmg -= shiledHitHpDef + m_strStat.m_fDef;
        }
        else
            dmg -= m_strStat.m_fDef;

        dmg = Mathf.Max(0, dmg);

        attacker.GetComponent<Character>().m_strAttack.SpecialAddAttackInfo();

        HitAnimation();

        Stop(0.2f);

        float NewHp = m_strStat.m_fHp - dmg;
        SetHp(NewHp);
    }

    // 공격 끝
    void AttackEnd()
    {
        foreach (var DetectorCollider in m_GoAttackItem)
            DetectorCollider.AttackCanOff();

        m_bCanAttack = true;
        m_bWaiting = false;

        m_strStat.m_fAtk = m_strStat.m_fOriginalAtk;
        StopCoroutine(CheckNextAttack());
        StartCoroutine(StaminaGraduallyFillingUp());
        eState = CreatureState.Idle;
    }

    IEnumerator AttackEndCheck()
    {
        AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo((int)Layers.BaseLayer);

        if (stateInfo.IsName(m_strAttack.info.m_sAnimName))
        {
            if (stateInfo.normalizedTime >= 0.9)
            {
                AttackEnd();
            }
        }

        yield return null;
    }

    // 애니메이션 이벤트에서 실행
    void ActionStateChange(string actionName)
    {
        m_strAttack.ActionStateChange(actionName);
    }

    protected void ActionStateEnd(string eState)
    {
        m_strAttack.ActionStateReset(eState);
    }

    void MoveEnable()
    {
        eState = CreatureState.Idle;
        m_bWaiting = false;
        m_strAttack.ActionStateReset();
    }

    public bool CheckCooltime()
    {
        if (m_fCoolTime == 0)
            return true;

        return false;
    }

    protected virtual IEnumerator CheckNextAttack()
    {
        AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo((int)Layers.BaseLayer);

        if (stateInfo.IsName(m_strAttack.info.m_sAnimName))
        {
            if (stateInfo.normalizedTime >= 0.6)
            {
                // 어떤 공격 키를 눌렀는지에 따라서
                if (m_bNextAttack == true)
                    AttackEvent(m_strAttack.info.m_iNextNum);
            }
        }

        yield return null;
    }
}
