using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class TrigerDetector : MonoBehaviour
{
    // 무기에 부착하는 컴포넌트
    // 무기가 타겟과 공격시 피격 함수 호출
    Character CAttacker = null;
    Character m_goTarget = null;
    Collider m_Collider = null;

    private void Start()
    {
        CAttacker = GetComponentInParent<Character>();

        m_Collider = GetComponent<Collider>();
        m_Collider.isTrigger = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        m_goTarget = other.GetComponent<Character>();

        if (m_goTarget != null)
        {
            CAttacker.m_goTarget = m_goTarget;
            CAttacker.m_goTarget.HitEvent(CAttacker, CAttacker.m_TotalAttack);
            m_goTarget = null;
            m_Collider.isTrigger = false;
        }
    }

    public void Attack()
    {
        m_Collider.isTrigger = true;
        Managers.Battle.EventDelegateAttackEnd += () => { m_Collider.isTrigger = false; };
    }
}
