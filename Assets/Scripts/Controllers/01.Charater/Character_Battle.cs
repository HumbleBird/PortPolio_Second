using System;
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

    protected Coroutine cAttackCheck;
    protected Coroutine cCheckNextAttack;

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
        StrAnimation(m_strAttack.info.m_sAnimName);

        // 공격 데미지 더해주기
        m_fCoolTime += m_strAttack.info.m_fCoolTime;
        m_strStat.m_fAtk += m_strAttack.info.m_fDmg;

        // 공격 종료 체크
        cAttackCheck = StartCoroutine(AttackEndCheck());
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

            // TODO
            m_strStat.m_fDef += shiledHitHpDef;
        }

        dmg = Mathf.Max(0, dmg - m_TotalDefence);
        float NewHp = m_strStat.m_fHp - dmg;
        SetHp(NewHp, attacker);

        // 공격 별 특수 효과
        attacker.GetComponent<Character>().m_strAttack.SpecialAddAttackInfo();

        HitAnimation();

        Stop(0.2f);
    }

    IEnumerator AttackEndCheck()
    {
        while (true)
        {
            AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo((int)AnimationLayers.BaseLayer);

            if (stateInfo.IsName(m_strAttack.info.m_sAnimName))
            {
                if (stateInfo.normalizedTime >= 0.9)
                {
                    AttackEnd();
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    // 공격 끝
    protected virtual void AttackEnd()
    {
        foreach (var DetectorCollider in m_GoAttackItem)
            DetectorCollider.AttackCanOff();

        m_bCanAttack = true;
        m_bWaiting = false;

        m_strStat.m_fAtk = m_strStat.m_fOriginalAtk;

        StopCoroutine(cAttackCheck);

        eState = CreatureState.Idle;
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
        AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo((int)AnimationLayers.BaseLayer);

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
