using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Shrine_Handmaid : NPC
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

        Managers.UI.ShowPopupUI<UI_Shop>();

    }
}
