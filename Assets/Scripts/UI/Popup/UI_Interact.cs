using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Interact : UI_Popup
{
	enum Texts
	{
		InteractText
	}

	enum Images
    {
		ItemImage
    }

	public TextMeshProUGUI m_InteratableText;

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		BindText(typeof(Texts));
		BindImage(typeof(Images));

		m_InteratableText = GetText((int)Texts.InteractText);

		return true;
	}

	public void SetInfo(string ItemName, string iconPath)
    {
		m_InteratableText.text = ItemName;
		Image image = GetImage((int)Images.ItemImage);
		image.sprite = Managers.Resource.Load<Sprite>(iconPath);
		image.enabled = true;
	}
}
