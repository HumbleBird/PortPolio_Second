using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigerDetector : MonoBehaviour
{
    // ���⿡ �����ϴ� ������Ʈ
    // ���Ⱑ Ÿ�ٰ� ���ݽ� �ǰ� �Լ� ȣ��
    GameObject m_gPlayer;

    public void Set()
    {
        // Managers.Camera.Shake(100001);
        m_gPlayer = transform.root.gameObject;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        Player pc = m_gPlayer.GetComponent<Player>();

        if(other != null)
            pc.Attack(other.gameObject);

        gameObject.SetActive(false);
    }
}
