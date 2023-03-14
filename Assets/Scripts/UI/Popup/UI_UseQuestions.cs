using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_UseQuestions : UI_Popup
{
	enum Texts
	{
		QestionsText,
		YesText,
		NoText,
	}

	public TextMeshProUGUI m_sQestion { get; private set; }
	public TextMeshProUGUI m_sYesText { get; private set; }
	public TextMeshProUGUI m_sNoText { get; private set; }

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		BindText(typeof(Texts));
		m_sQestion = GetText((int)Texts.QestionsText);
		m_sYesText = GetText((int)Texts.YesText);
		m_sNoText = GetText((int)Texts.NoText);

		return true;
	}

	public void SetQeustion(string qestion)
    {
		m_sQestion.text = qestion;
	}

	public override void RefreshUI()
	{
		base.RefreshUI();

		// 나중에 언어 변경
	}
}
