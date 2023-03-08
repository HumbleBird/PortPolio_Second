using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Shrine_Handmaid : NPC
{
    // 상점 기능 NPC
    public override void StartInteraction()
    {
        base.StartInteraction();
    }

    public override void NextInteraction()
    {
        base.NextInteraction();

        Debug.Log("상점 오픈");
        Managers.UIBattle.ShowAndClose(Managers.UIBattle.UIGameScene.UIShop);

        // 이제 esc를 감지하여 팝업창 닫기
    }

    public override void EndInteraction()
    {
        base.EndInteraction();
    }
}
