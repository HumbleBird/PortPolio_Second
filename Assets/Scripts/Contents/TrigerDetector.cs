using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class TrigerDetector : MonoBehaviour
{
    // ���⿡ �����ϴ� ������Ʈ
    // ���Ⱑ Ÿ�ٰ� ���ݽ� �ǰ� �Լ� ȣ��
    GameObject Attacker = null;
    Character CAttacker = null;
    public Collider m_cAttackCollider;
    public AttackCollider eAttackCollider = AttackCollider.None;

    public void Init()
    {
        CAttacker = transform.GetComponentInParent<Character>();
        CAttacker.m_GoAttackItem.Add(GetComponent<TrigerDetector>());

        Attacker = CAttacker.gameObject;

        m_cAttackCollider = GetComponent<Collider>();
        m_cAttackCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.tag != "Ground")
        {
            CAttacker.m_goTarget = other.gameObject;
            CAttacker.m_goTarget.GetComponent<Character>().HitEvent(Attacker, CAttacker.m_TotalAttack);
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
