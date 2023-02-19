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
        if (other != null && other.tag != "Ground")
        {
            CAttacker.m_goTarget = other.GetComponent<Character>();
            CAttacker.m_goTarget.HitEvent(CAttacker.gameObject, CAttacker.m_TotalAttack);
        }
    }

    public void AttackCanOn()
    {
        m_cAttackCollider.enabled = true;

    }

    public void AttackCanOff()
    {
        m_cAttackCollider.enabled = false;
    }
}
