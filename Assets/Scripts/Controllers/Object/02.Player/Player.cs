using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : Character
{
    protected override void Init()
    {
        base.Init();

        StartCoroutine(UpdateCoolTime());
        StartCoroutine(StaminaGraduallyFillingUp());
    }
}
