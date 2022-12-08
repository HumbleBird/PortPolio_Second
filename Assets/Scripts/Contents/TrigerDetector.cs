using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigerDetector : MonoBehaviour
{
    // ���⿡ �����ϴ� ������Ʈ
    // ���Ⱑ Ÿ�ٰ� ���ݽ� �ǰ� �Լ� ȣ��
    GameObject m_gAttacker = null;
    public Collider m_cAttackCollider;

    private void Start()
    {
        // Managers.Camera.Shake(100001);
        m_gAttacker = transform.root.gameObject;
        m_gAttacker.GetComponent<Character>().m_GoAttackItem = gameObject.GetComponent<TrigerDetector>();

        m_cAttackCollider = GetComponent<Collider>();
        m_cAttackCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            m_gAttacker.GetComponent<Character>().m_goTarget = other.gameObject;
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
