using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigerDetector : MonoBehaviour
{
    // ���⿡ �����ϴ� ������Ʈ
    // ���Ⱑ Ÿ�ٰ� ���ݽ� �ǰ� �Լ� ȣ��
    GameObject m_gPlayer;

    private void Start()
    {
        // Managers.Camera.Shake(100001);
        m_gPlayer = transform.root.gameObject;
        Character ch = m_gPlayer.GetComponent<Character>();
        ch.m_GoAttackItem = gameObject;

        gameObject.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        Player pc = m_gPlayer.GetComponent<Player>();

        if (other != null)
            pc.AttackEvent(other.gameObject);

        gameObject.SetActive(false);
    }
}
