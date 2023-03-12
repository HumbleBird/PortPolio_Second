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
        StopCoroutine(Managers.Battle.IStandAction());
    }

    public override void StartInteraction()
    {
        base.StartInteraction();

        Managers.UIBattle.ShowAndCloseUI<UI_Shop>();
        Managers.Battle.PlayerCanMove(false);

        StartCoroutine(Managers.Battle.IStandAction(
        () => {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                EndInteraction();
                Managers.UIBattle.ShowAndCloseUI<UI_Shop>();
                StopStandInputkey();
            }
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
