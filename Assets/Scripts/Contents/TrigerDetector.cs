using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigerDetector : MonoBehaviour
{
    // ���⿡ �����ϴ� ������Ʈ
    // ���Ⱑ Ÿ�ٰ� ���ݽ� �ǰ� �Լ� ȣ��
    GameObject Attacker = null;
    Character CAttacker = null;
    public Collider m_cAttackCollider;

    private void Start()
    {
        // Managers.Camera.Shake(100001);
        Attacker = transform.root.gameObject;
        CAttacker = Attacker.GetComponent<Character>();
        CAttacker.m_GoAttackItem = GetComponent<TrigerDetector>();

        m_cAttackCollider = GetComponent<Collider>();
        m_cAttackCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            CAttacker.m_goTarget = other.gameObject;
            CAttacker.AttackEvent();
        }
    }

    public void AttackcCanOn()
    {
        m_cAttackCollider.enabled = true;

    }

    public void AttackCanOff()
    {
        m_cAttackCollider.enabled = false;

    }
}
