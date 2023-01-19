using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Player : Character
{
    public Inventory m_Inven { get; private set; } = new Inventory();
    protected Coroutine cStaminaGraduallyFillingUp;
    protected override void Init()
    {
        base.Init();

        eCreatureType = CreatureType.Player;
        tag = "Player";

        cStaminaGraduallyFillingUp = StartCoroutine(StaminaGraduallyFillingUp());
    }

    protected override void UpdateSkill()
    {
        base.UpdateSkill();

        cCheckNextAttack = StartCoroutine(CheckNextAttack());
    }
}
