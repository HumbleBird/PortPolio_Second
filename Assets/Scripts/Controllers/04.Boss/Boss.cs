using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public partial class Boss : Monster
{
    public Boss()
    {
        eObjectType = ObjectType.Boss;
        gameObject.tag = "Boss";
    }

    protected override IEnumerator AttackPattern()
    {
        throw new System.NotImplementedException();
    }
}
