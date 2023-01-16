using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Player : Character
{
	protected override void UpdateSkill()
	{
		base.UpdateSkill();

		StartCoroutine(CheckNextAttack());
	}

	protected override void AttackEvent(int id)
    {
        base.AttackEvent(id);

		// 스테미너 일시 정지
		StartCoroutine(StaminaGraduallyFillingUp(false));
	}

	protected override void SetHp(float NewHp)
	{
		base.SetHp(NewHp);

		Managers.UIBattle.StatUIRefersh();
	}

	protected override void SetStemina(float NewSetStamina)
	{
		base.SetStemina(NewSetStamina);

		Managers.UIBattle.StatUIRefersh();
	}

}
