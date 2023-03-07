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
    }

    public override void EndInteraction()
    {
        base.EndInteraction();
    }
}
