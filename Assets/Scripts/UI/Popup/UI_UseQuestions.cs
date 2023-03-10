using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_UseQuestions : UI_Popup
{
	enum Texts
	{
		QestionsText,
		YesText,
		NoText,
	}

	string m_sQestion = null;

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		BindText(typeof(Texts));

		return true;
	}

	public void SetQeustion(string qestion)
    {
		m_sQestion = qestion;
	}

	public override void RefreshUI()
	{
		base.RefreshUI();

		// 나중에 언어 변경
	}
}
