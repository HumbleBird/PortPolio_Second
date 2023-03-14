using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_BGGameInfo : UI_Base
{
    enum Texts
    {
        UINameText,
        HoldingGoldCountText
    }

    public TextMeshProUGUI m_iHaveMoeny { get; private set; }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        m_iHaveMoeny = GetText((int)Texts.HoldingGoldCountText);

        return true;
    }

    public override void RefreshUI()
    {
        base.RefreshUI();

        m_iHaveMoeny.text = Managers.Object.myPlayer.m_iHaveMoeny.ToString();
    }
}
