using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class AI : Charater
{
    public override void HitEvent()
    {
        m_CaughtPlayer = false;
        if (target != null)
            Managers.Battle.HitEvent(gameObject, (int)Atk, target);
    }
}