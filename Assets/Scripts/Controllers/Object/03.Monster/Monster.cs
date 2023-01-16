using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public partial class Monster : AI
{
    protected override void Init()
    {
        base.Init();

        tag = "Monster";
        eCreatureType = CreatureType.Monster;
    }
}
