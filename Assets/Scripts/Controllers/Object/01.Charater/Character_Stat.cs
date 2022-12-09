using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Character : Base
{
    // 스탯 변화

    private void SetHp(int NewHp)
    {
        Hp = NewHp;
        if (Hp < 0)
            Hp = 0;

        Managers.UIBattle.StatUIRefersh();
        StartCoroutine(Managers.UIBattle.UIPlayerInfo.DownHP());
    }

    private void SetStamina(int NewSetStamina)
    {
        Stamina = NewSetStamina;
        if (Stamina < 0)
            Stamina = 0;

        Managers.UIBattle.StatUIRefersh();
    }

    protected void StaminaGraduallyFillingUp()
    {
        Stamina = (int)(Stamina + Time.deltaTime);
        Managers.UIBattle.StatUIRefersh();
    }
}
