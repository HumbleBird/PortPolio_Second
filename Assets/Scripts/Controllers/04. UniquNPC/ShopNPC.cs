using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ShopNPC : NPC, IShop
{
    // 상점 기능 NPC
    public override void Talk()
    {
        base.Talk();

        m_dialogue.StartDialogue(1);
    }

    public override void Interaction()
    {
        base.Interaction();

        Managers.UIBattle.ShowAndCloseUI<UI_Shop>();

        StartCoroutine(Managers.UIBattle.DelegateShowAndClose(() => 
        {
            Managers.UIBattle.ShowAndCloseUI<UI_Shop>();
            Managers.UI.ShowPopupUI<UI_SelectWindow>();
            StartCoroutine(Managers.Battle.NPCInteractionEventFunction());
        }));
    }

    public void BuyItem(int id)
    {
        throw new NotImplementedException();
    }
    
    public void SellItem(int id)
    {
        throw new NotImplementedException();
    }
}
