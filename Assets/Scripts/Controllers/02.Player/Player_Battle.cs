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

		// 스테미너 감소
		m_strStat.m_fStemina -= 10;
		SetStemina(m_strStat.m_fStemina);

		// 스테미너 일시 정지
		StopCoroutine(cStaminaGraduallyFillingUp);
	}

	protected override void AttackEnd()
    {
		base.AttackEnd();

		StartCoroutine(StaminaGraduallyFillingUp());
		m_goTarget = null;
	}

	protected virtual IEnumerator StaminaGraduallyFillingUp()
	{
        while (true)
        {
			m_strStat.m_fStemina += 0.2f;
			SetStemina(m_strStat.m_fStemina);

			yield return null;
		}
	}

	protected override void HowNextAttack()
	{
		base.HowNextAttack();

		// 플레이어라면 직접 할지 말지를 결정함.
		m_bNextAttack = true;
	}
}
