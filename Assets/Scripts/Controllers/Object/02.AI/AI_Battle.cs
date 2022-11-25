using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class AI : Charater
{
    public virtual void Attack()
    {

    }

    public override void HitEvent(GameObject attacker, float dmg)
    {
        m_CaughtPlayer = false;
        if (target != null)
        {
            Charater player = target.GetComponent<Charater>();
            player.HitEvent(gameObject, (int)Atk);
        }
    }
}