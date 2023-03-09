using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SelectWindow : UI_Popup
{
	enum Texts
	{
		TalkText,
		InteractionText
	}

	enum Images
	{
		TalkButtonImage,
		InteractionButtonImage
	}
	

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		BindText(typeof(Texts));
		BindImage(typeof(Images));

		return true;
	}
}
