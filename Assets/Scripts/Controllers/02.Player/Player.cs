using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Player : Character
{
    protected Coroutine cStaminaGraduallyFillingUp;
    protected override void Init()
    {
        base.Init();

        eObjectType = ObjectType.Player;
        tag = "Player";

        cStaminaGraduallyFillingUp = StartCoroutine(StaminaGraduallyFillingUp());
    }

    protected override void UpdateSkill()
    {
        base.UpdateSkill();

        cCheckNextAttack = StartCoroutine(CheckNextAttack());
    }
}
