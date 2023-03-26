using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public partial class Boss : Monster
{
    public new ObjectType eObjectType= ObjectType.Boss;


    protected override IEnumerator AttackPattern()
    {
        throw new System.NotImplementedException();
    }
}
