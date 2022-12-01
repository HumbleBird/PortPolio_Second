using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Character : Base
{
    public GameObject target { get; set; } // 타겟

    public float m_fCoolTime = 0f;
    public bool _isNextCanAttack = true;

    protected Attack _attack;

    public void ChangeClass(string typeClass)
    {
        switch (typeClass)
        {
            case "Blow":
                _attack = new Blow();
                break;
            case "Range":
                _attack = new Range();
                break;
            default:
                break;
        }
    }

    // 공격
    public virtual void Attack(GameObject target)
    {
        Character ct = target.GetComponent<Character>();
        if(ct != null)
        {
            // 데미지 계산
            // 크리티컬
            ct.HitEvent(gameObject, Atk);
        }
    }

    // 피격
    public virtual void HitEvent(GameObject attacker, float dmg)
    {
        // 데미지 계산 및 체력 감소
        int damage = (int)Mathf.Max(0, dmg - Def);
        Hp -= damage;

        // 히트 애니메이션
        Animator.Play("Hit");

        // 정지 및 무적 시간
        Stop(0.2f);

        // 사망 체크
        if (Hp <= 0)
        {
            Hp = 0;
            State = Define.CreatureState.Dead;
        }
    }

    public virtual void CanNextAttack(int id)
    {
        // 애니메이션 자동 해제
        Table_Attack.Info info = Managers.Table.m_Attack.Get(id);
        Animator.SetBool(info.m_sAnimName, false);

        State = CreatureState.Idle;
    }
}
