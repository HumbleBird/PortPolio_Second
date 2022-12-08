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

    // 무기 이벤트에서 콜라이더를 활성화 시켜주기 위해
    public virtual void Attack()
    {
        m_GoAttackItem.AttackcCanOn();

        AttackEvent();
    }

    // 공격 이벤트
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

    private void SetHp(int NewHp)
    {
        Hp = NewHp;
        if (Hp < 0)
            Hp = 0;

        RefreshUI();
    }

    // 피격
    public virtual void HitEvent(GameObject attacker, float dmg)
    {
        if(m_actionState == ActionState.Shield)
        {
            int ShiledHitStamina = 10;
            float shiledHitHpDef = 3.0f; // 체력 피격 데미지 감소율

            Stamina -= ShiledHitStamina;
            dmg = (float)dmg % shiledHitHpDef;
        }
        else
        {
            dmg = (int)Mathf.Max(0, dmg - Def);
        }

        int NewHp = Hp - (int)dmg;
        SetHp(NewHp);

        if(m_actionState == ActionState.Shield)
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
            State = Define.CreatureState.Dead;
        }
    }

    public virtual void AttackEnd(int id)
    {
        // 애니메이션 setbool을 false로
        // 다음 콤보 공격 번호가 있다면
        // 콜라이더 해제
        // Character State를 Idle로

        Table_Attack.Info info = Managers.Table.m_Attack.Get(id);
        Animator.SetBool(info.m_sAnimName, false);

        m_GoAttackItem.AttackCanOff();

        State = CreatureState.Idle;
    }
}
