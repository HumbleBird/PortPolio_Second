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
		TalkText,
		InteractionText
	}

	enum Images
    {
		DialoguePanelImage,
		TalkButtonImage,
		InteractionImage
	}

	Queue<string> m_sentences = new Queue<string>();
	Image m_InteractionImage;
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
		m_InteractionImage = GetImage((int)Images.InteractionImage);

		m_DialoguePannelImage.gameObject.BindEvent(() =>
		{
			DisplayNextSentence();
		});

		return true;
	}

	public void DisplayNextSentence(Queue<string> sentences)
	{
		m_sentences = sentences;

		m_DialogueText.gameObject.SetActive(true);
		m_DialoguePannelImage.gameObject.SetActive(true);
		m_InteractionImage.gameObject.SetActive(false);

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
		Debug.Log("다음 상태로");

		m_DialogueText.gameObject.SetActive(false);
		m_DialoguePannelImage.gameObject.SetActive(false);
		m_InteractionImage.gameObject.SetActive(true);

		StartCoroutine(Managers.Battle.NPCInteractionEventFunction());
	}
}
