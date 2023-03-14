using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    // Diarogue
    protected Dialogue m_dialogue = new Dialogue();
    IEnumerator m_IEnumerator = null;

    // 코루틴 정보 set
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

    // 대화 선택시
    public virtual void Talk()
    {
        Managers.UI.ClosePopupUI();
        Managers.Battle.PlayerCanMove(false);

    }

    // 상호작용 선택시
    public virtual void Interaction()
    {
        Managers.UI.ClosePopupUI();
        Managers.Battle.PlayerCanMove(false);

    }

    // NPC와의 상호작용을 다 끝낸다면 (나가기)
    public virtual void EndMeetPlayer()
    {
        Managers.UI.ClosePopupUI();
        Managers.Battle.PlayerCanMove();
        Managers.Battle.EVENTFunction -= InputButtonSelect;
        Debug.Log("NPC 상호작용 끝");
    }
}
