using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Player : Character
{
    public override void UpdateSound()
    {
        if (eState == CreatureState.Dead)
            SoundPlay("Player" + eState.ToString());
    }
}
