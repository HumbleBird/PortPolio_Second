﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Character : Base
{
    public GameObject m_goTarget { get; set; } // 타겟

    public float m_fCoolTime = 0f;
    public bool _isNextCanAttack = true;

    protected Attack m_strAttack = new Attack();

    public int m_iBasicAttackNum = 1;
    public int m_iStrongAttackNum = 4;
    public int m_iKickNum = 501;

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

    // 공격 시도
    public virtual void Attack(int id)
    {
        m_goTarget = null;
        AttackCollider tempAttackCollider = AttackCollider.None;

        // 콜라이더 활성화
        if (id == 501) // TODO
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

        m_strAttack.AttackInfoCal(id);
    }

    // 공격 판정 체크
    public virtual void AttackEvent()
    {
        if (m_goTarget!= null)
        {
            Debug.Log("Attack : " + m_goTarget.name);

            m_strAttack.SpecialAddAttackInfo();

            Character ct = m_goTarget.GetComponent<Character>();
            if (ct != null)
            {
                // TODO 크리티컬
                ct.HitEvent(gameObject, Atk);
            }
        }
    }

    // 피격
    public virtual void HitEvent(GameObject attacker, float dmg)
    {
        if (eActionState == ActionState.Shield)
        {
            int ShiledHitStamina = 10;
            float shiledHitHpDef = 1.0f; // 체력 피격 데미지 감소율

            Stamina -= ShiledHitStamina;
            dmg = (float)dmg % shiledHitHpDef;
        }
        else if (eActionState == ActionState.Invincible)
            return;
        else
        {
            dmg = (int)Mathf.Max(0, dmg - Def);
        }

        if(eActionState == ActionState.Shield)
        {
            Animator.SetTrigger("Hit");
        }
        else
        {
            Animator.Play("Hit");
        }

        Stop(0.2f);

        int NewHp = Hp - (int)dmg;
        SetHp(NewHp);

        if (Hp <= 0)
        {
            Hp = 0;
            eState = Define.CreatureState.Dead;
        }
    }

    // 공격 끝
    public virtual void AttackEnd()
    {
        _isNextCanAttack = true;

        foreach (var DetectorCollider in m_GoAttackItem)
            DetectorCollider.AttackCanOff();

        eState = CreatureState.Idle;
    }

    void ActionStateChange(string actionName)
    {
        m_strCharacterAction.ActionStateChange(actionName);
    }

    protected void ActionStateEnd()
    {
        m_strCharacterAction.ActionStateReset();
    }
}
