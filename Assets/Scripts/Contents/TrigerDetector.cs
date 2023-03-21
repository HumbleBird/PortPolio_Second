using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class TrigerDetector : MonoBehaviour
{
    // 무기에 부착하는 컴포넌트
    // 무기가 타겟과 공격시 피격 함수 호출
    Character CAttacker = null;
    public Collider m_cAttackCollider;
    public AttackCollider eAttackCollider = AttackCollider.None;

    public void Init()
    {
        CAttacker = transform.GetComponentInParent<Character>();
        CAttacker.m_GoAttackItem.Add(GetComponent<TrigerDetector>());

        m_cAttackCollider = GetComponent<Collider>();
        m_cAttackCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponent<Character>();

        if (character != null)
        {
            CAttacker.m_goTarget = character;
            CAttacker.m_goTarget.HitEvent(character, CAttacker.m_TotalAttack);
            AttackCan(false);
        }
    }

    public void AttackCan(bool b)
    {
        m_cAttackCollider.enabled = b;

    }
}
