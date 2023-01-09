using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Character : Base
{
    public GameObject m_goTarget { get; set; } // 타겟

    public float m_fCoolTime = 0f;
    public bool m_bCanAttack = true;
    public bool m_bNextAttack = false;

    protected Attack m_strAttack = new Attack();

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

    public void AttackEvent(int id)
    {
        m_strAttack.CheckCooltime();

        m_strAttack.info = Managers.Table.m_Attack.Get(id);

        if (m_strAttack.info == null)
        {
            Debug.LogError($"해당하는 {id}의 스킬이 없습니다.");
            return;
        }

        m_bCanAttack = false;
        eState = CreatureState.Skill;

        // 스테미너 일시 정지
        SetStaminaGraduallyFillingUp(false);

        // 애니메이션 실행
        StrAnimation(m_strAttack.info.m_sAnimName);
        

        // 공격 데미지 더해주기
        m_fCoolTime = m_strAttack.info.m_fCoolTime;
        Atk += m_strAttack.info.m_fDmg;
    }

    void Attack()
    {

        WeaponColliderOn();
    }

    void WeaponColliderOn()
    {
        int id = m_strAttack.info.m_nID;

        m_goTarget = null;
        AttackCollider tempAttackCollider = AttackCollider.None;

        // 콜라이더 활성화
        if (id == m_strAttack.m_iKickNum) // TODO
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
        Character ct = m_goTarget.GetComponent<Character>();
        if (ct == null)
            return;

        m_strAttack.SpecialAddAttackInfo();

        if (eActionState == ActionState.Invincible)
            return;
        else if (eActionState == ActionState.Shield)
        {
            int ShiledHitStamina = 10;
            float shiledHitHpDef = 1.0f; // 체력 피격 데미지 감소율

            Stamina -= ShiledHitStamina;
            dmg = (float)dmg % shiledHitHpDef;
        }
        else
            dmg = (int)Mathf.Max(0, dmg - Def);

        HitAnimation();

        Stop(0.2f);

        int NewHp = Hp - (int)dmg;
        SetHp(NewHp);
    }

    // 공격 끝
    public virtual void AttackEnd()
    {
        m_bCanAttack = true;

        foreach (var DetectorCollider in m_GoAttackItem)
            DetectorCollider.AttackCanOff();

        m_bWaiting = false;
        SetStaminaGraduallyFillingUp(true);

        m_strAttack.AttackAtkReset();
        eState = CreatureState.Idle;
    }

    // 애니메이션 이벤트에서 실행
    void ActionStateChange(string actionName)
    {
        m_strCharacterAction.ActionStateChange(actionName);
    }

    protected void ActionStateEnd(string eState)
    {
        m_strCharacterAction.ActionStateReset(eState);
    }

    void MoveEnable()
    {
        eState = CreatureState.Idle;
        m_bWaiting = false;
        m_strCharacterAction.ActionStateReset();
    }
}
