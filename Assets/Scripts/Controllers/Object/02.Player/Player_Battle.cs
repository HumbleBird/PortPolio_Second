using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Player : Character
{
	protected override void AttackEvent(int id)
    {
        base.AttackEvent(id);
		eState = CreatureState.Skill;

		// 스테미너 감소
		m_strStat.m_fStemina -= 10f;
		SetStemina(m_strStat.m_fStemina);

		// 스테미너 일시 정지
		StopCoroutine(cStaminaGraduallyFillingUp);
	}

	protected override void SetHp(float NewHp)
	{
		base.SetHp(NewHp);

		//Managers.UIBattle.StatUIRefersh();
	}

	protected override void SetStemina(float NewSetStamina)
	{
		base.SetStemina(NewSetStamina);

		//Managers.UIBattle.StatUIRefersh();
	}

	protected override void AttackEnd()
    {
		base.AttackEnd();

		StartCoroutine(StaminaGraduallyFillingUp());
		StopCoroutine(cCheckNextAttack);
		m_goTarget = null;

	}
}
