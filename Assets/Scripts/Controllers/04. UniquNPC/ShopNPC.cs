using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ShopNPC : NPC
{
    // 상점 기능 NPC
    public override void Talk()
    {
        base.Talk();

        m_dialogue.StartDialogue(1);

        // TODO Q를 누르면 대화에서 나가 선택창으로 변경
    }

    public override void Interaction()
    {
        base.Interaction();

        Managers.UIBattle.ShowAndCloseUI<UI_Shop>();

        StartCoroutine(IDownShop());
    }

    public IEnumerator IDownShop()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Managers.UIBattle.ShowAndCloseUI<UI_Shop>();
                Managers.UI.ShowPopupUI<UI_SelectWindow>();
                Managers.Battle.EVENTFunction();
                yield break;
            }

            yield return null;
        }
    }
}
