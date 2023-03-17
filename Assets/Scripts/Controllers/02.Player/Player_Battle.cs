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

		cStaminaGraduallyFillingUp = StartCoroutine(StaminaGraduallyFillingUp());
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


}
