using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public partial class Monster : AI
{
    List<int> m_rewardsId;

    protected override void Init()
    {
        base.Init();

        tag = "Monster";
        eCreatureType = CreatureType.Monster;

        // Temp
        
    }

    public override void OnDead(GameObject Attacker)
    {
        base.OnDead(Attacker);


    }
}
