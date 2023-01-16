using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Player : Character
{
    protected override void Init()
    {
        base.Init();

        eCreatureType = CreatureType.Player;
        tag = "Player";


        StartCoroutine(UpdateCoolTime());
        StartCoroutine(StaminaGraduallyFillingUp());
    }
}
