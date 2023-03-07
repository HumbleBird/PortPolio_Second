using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UI_Inven : UI_Base
{
    public UI_InvenMain UIInvenMain { get; set; }
    public UI_ItemDes UIItemDes { get; set; }
    public UI_CharacterData UICharacterData { get; set; }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        UIInvenMain = GetComponentInChildren<UI_InvenMain>();
        UIItemDes = GetComponentInChildren<UI_ItemDes>();
        UICharacterData = GetComponentInChildren<UI_CharacterData>();

        gameObject.SetActive(false);

        return true;
    }


    public void RefreshUI()
    {
        if (_init == false)
            return;

        UIInvenMain.RefreshUI();
        UIItemDes.RefreshUI();

        UICharacterData.SetInfo(Managers.Object.myPlayer.gameObject);
        UICharacterData.RefreshUI();
    }
}
