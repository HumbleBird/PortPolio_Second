using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    // Diarogue
    Dialogue m_dialogue = new Dialogue();

    // 무조건 첫 대화
    public virtual void StartInteraction()
    {
        m_dialogue.CloseDialogue();
        m_dialogue.StartDialogue(1);
    }

    // 첫 대사 끝나면 상호작용
    public virtual void NextInteraction()
    {
        // NPC 값에 따라 상점, 퀘스트, 업그레이드

        // 여기서는 상점 오픈

        m_dialogue.CloseDialogue();
    }

    // 모든 상호작용이 끝나면
    public virtual void EndInteraction()
    {
        m_dialogue.CloseDialogue();

        Managers.Object.myPlayer.m_bWaiting = false;
        Managers.Object.myPlayer.m_bIsNPCInteracting = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
