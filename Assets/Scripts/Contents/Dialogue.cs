using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{
	private Queue<string> sentences = new Queue<string>();
	UI_Dialogue uiDialogue;

	public void StartDialogue(int id)
	{
		sentences.Clear();

		Table_Dialogue.Info info;

		info = Managers.Table.m_Dialogue.Get(id);
		if (info == null)
			return;

		sentences.Enqueue(info.m_iTextEnglish);

		while (info.m_iNextIdx != 0)
        {
			info = Managers.Table.m_Dialogue.Get(info.m_iNextIdx);
			sentences.Enqueue(info.m_iTextEnglish);
		}

		uiDialogue = Managers.UI.ShowPopupUI<UI_Dialogue>();
		uiDialogue.DisplayNextSentence(sentences);

		Managers.Object.myPlayer.m_bWaiting = true;
		Managers.Object.myPlayer.eState = Define.CreatureState.Idle;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	public void CloseDialogue()
    {
		if(uiDialogue != null)
			uiDialogue.ClosePopupUI();
	}
}
