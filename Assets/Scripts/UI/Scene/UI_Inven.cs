using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UI_Inven : UI_Base
{

    public UI_InvenMain UIInvenMain { get; private set; }
    public UI_ItemDes UIItemDes { get; private set; }
    public UI_CharacterData UICharacterData { get; private set; }
    public UI_BGGameInfo UIBGGameInfo { get; private set; }


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        UIInvenMain = GetComponentInChildren<UI_InvenMain>();
        UIItemDes = GetComponentInChildren<UI_ItemDes>();
        UICharacterData = GetComponentInChildren<UI_CharacterData>();
        UIBGGameInfo = GetComponentInChildren<UI_BGGameInfo>();

        gameObject.SetActive(false);

        return true;
    }

    public override void RefreshUI()
    {
        base.RefreshUI();

        UIInvenMain.RefreshUI();
        UIItemDes.RefreshUI();
        UICharacterData.RefreshUI();
        UIBGGameInfo.RefreshUI();
    }
}
