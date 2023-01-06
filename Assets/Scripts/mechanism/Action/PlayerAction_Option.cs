using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

public partial class PlayerAction : Strategy
{
    void ShowInputKeySetting()
    {
        ShowAndClose(Managers.UIBattle.UISetting);
    }

    void ShowInventory()
    {
        ShowAndClose(Managers.UIBattle.UIInven);
    }

    void ShowAndClose(UI_Popup popup)
    {
        bool B = popup.gameObject.activeSelf;
        popup.gameObject.SetActive(!B);
        if (B == true)
            m_cGo.m_bWaiting = false;
    }
}
