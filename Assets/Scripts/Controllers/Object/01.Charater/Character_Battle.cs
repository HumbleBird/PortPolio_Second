using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Character : Base
{
    public GameObject m_goTarget { get; set; } // 타겟

    public float m_fCoolTime = 0f;
    public bool _isNextCanAttack = true;

    protected Attack m_strAttack;

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
    public virtual void Attack()
    {
        m_goTarget = null; // 타겟 초기화

        m_GoAttackItem.AttackcCanOn();
    }

    // 공격 판정 체크
    public virtual void AttackEvent()
    {
        if (m_goTarget!= null)
        {
            Debug.Log("Attack : " + m_goTarget.name);

            Character ct = m_goTarget.GetComponent<Character>();
            if (ct != null)
            {
                // 데미지 계산
                // 크리티컬
                ct.HitEvent(gameObject, Atk);
            }
        }
    }



    // 피격
    public virtual void HitEvent(GameObject attacker, float dmg)
    {
        if(eActionState == ActionState.Shield)
        {
            int ShiledHitStamina = 10;
            float shiledHitHpDef = 1.0f; // 체력 피격 데미지 감소율

            Stamina -= ShiledHitStamina;
            dmg = (float)dmg % shiledHitHpDef;
        }
        else
        {
            dmg = (int)Mathf.Max(0, dmg - Def);
        }

        int NewHp = Hp - (int)dmg;
        SetHp(NewHp);

        if(eActionState == ActionState.Shield)
        {
            Animator.SetTrigger("Hit");
        }
        else
        {
            Animator.Play("Hit");
        }

        Stop(0.2f);

        // TODO 무적시간

        if (Hp <= 0)
        {
            Hp = 0;
            eState = Define.CreatureState.Dead;
        }
    }

    public virtual void AttackEnd(int id)
    {
        // 애니메이션 setbool을 false로
        // 다음 콤보 공격 번호가 있다면
        // 콜라이더 해제
        // Character eState를 Idle로

        Table_Attack.Info info = Managers.Table.m_Attack.Get(id);
        Animator.SetBool(info.m_sAnimName, false);
        Atk -= info.m_fDmg;

        m_GoAttackItem.AttackCanOff();

        eState = CreatureState.Idle;
    }

    void ActionStateChange(string actionName)
    {
        m_strCharacterAction.ActionStateChange(actionName);
    }
}
