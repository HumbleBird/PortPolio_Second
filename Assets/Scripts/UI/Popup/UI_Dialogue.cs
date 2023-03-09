using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Dialogue : UI_Popup
{
	enum Texts
    {
		DialogueText,
	}

	enum Images
    {
		DialoguePanelImage,
	}

	Queue<string> m_sentences = new Queue<string>();
	Image m_DialoguePannelImage;
	TextMeshProUGUI m_DialogueText;

	public override bool Init()
    {
		if (base.Init() == false)
			return false;

		BindText(typeof(Texts));
		BindImage(typeof(Images));

		m_DialogueText = GetText((int)Texts.DialogueText);
		m_DialoguePannelImage = GetImage((int)Images.DialoguePanelImage);

		m_DialoguePannelImage.gameObject.BindEvent(() =>
		{
			DisplayNextSentence();
		});

		return true;
	}

	public void DisplayNextSentence(Queue<string> sentences)
	{
		m_sentences = sentences;

		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if (m_sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = m_sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence(string sentence)
	{
		m_DialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			m_DialogueText.text += letter;
			yield return null;
		}
	}

	public void EndDialogue()
	{
		TalkStart(false);

		StartCoroutine(Managers.Battle.NPCInteractionEventFunction());
	}

	void TalkStart(bool isTalk = true)
    {
		m_DialogueText.enabled = isTalk;
		m_DialoguePannelImage.enabled = isTalk;
	}
}
