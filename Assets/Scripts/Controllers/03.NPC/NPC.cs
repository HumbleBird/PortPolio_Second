using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    // Diarogue
    protected Dialogue m_dialogue = new Dialogue();
    IEnumerator m_IEnumerator = null;

    // �ڷ�ƾ ���� set
    public void InputButtonSelect()
    {
        m_IEnumerator = IInputFunction();
        StartCoroutine(m_IEnumerator);
    }

    public void EndButtonSelect()
    {
        StopCoroutine(m_IEnumerator);
    }

    IEnumerator IInputFunction()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Talk();
                yield break;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Interaction();
                yield break;
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                EndMeetPlayer();
                yield break;
            }

            yield return null;
        }
    }

    // ��ȭ ���ý�
    public virtual void Talk()
    {
        Managers.UI.ClosePopupUI();
        Managers.Battle.PlayerCanMove(false);

    }

    // ��ȣ�ۿ� ���ý�
    public virtual void Interaction()
    {
        Managers.UI.ClosePopupUI();
        Managers.Battle.PlayerCanMove(false);

    }

    // NPC���� ��ȣ�ۿ��� �� �����ٸ� (������)
    public virtual void EndMeetPlayer()
    {
        Managers.UI.ClosePopupUI();
        Managers.Battle.PlayerCanMove();
        Managers.Battle.EVENTFunction -= InputButtonSelect;
        Debug.Log("NPC ��ȣ�ۿ� ��");
    }
}
