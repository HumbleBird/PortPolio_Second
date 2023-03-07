using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    // Diarogue
    Dialogue m_dialogue = new Dialogue();

    // ������ ù ��ȭ
    public virtual void StartInteraction()
    {
        m_dialogue.CloseDialogue();
        m_dialogue.StartDialogue(1);
    }

    // ù ��� ������ ��ȣ�ۿ�
    public virtual void NextInteraction()
    {
        // NPC ���� ���� ����, ����Ʈ, ���׷��̵�

        // ���⼭�� ���� ����

        m_dialogue.CloseDialogue();
    }

    // ��� ��ȣ�ۿ��� ������
    public virtual void EndInteraction()
    {
        m_dialogue.CloseDialogue();

        Managers.Object.myPlayer.m_bWaiting = false;
        Managers.Object.myPlayer.m_bIsNPCInteracting = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
