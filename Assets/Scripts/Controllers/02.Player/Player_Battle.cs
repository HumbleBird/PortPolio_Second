using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Player : Character
{
	protected bool m_bNextAttack = false;
	protected Coroutine m_coAttackCheck;

	protected override void AttackEvent(int id)
    {
        base.AttackEvent(id);

		eState = CreatureState.Skill;

		// 스테미너 감소
		float newStemina = m_Stat.m_fStemina - 10;
		SetStemina(newStemina);

		// 스테미너 일시 정지
		StopCoroutine(cStaminaGraduallyFillingUp);

		// 공격 종료 체크
		m_coAttackCheck = StartCoroutine(CoAttackCheck());
	}

	protected override void AttackEnd()
    {
		base.AttackEnd();
		eState = CreatureState.Idle;
		m_bCanAttack = true;
		m_bNextAttack = false;
		cStaminaGraduallyFillingUp = StartCoroutine(StaminaGraduallyFillingUp());
	}

	protected IEnumerator StaminaGraduallyFillingUp()
	{
        while (true)
        {
			m_Stat.m_fStemina += 0.2f;
			SetStemina(m_Stat.m_fStemina);

			yield return null;
		}
	}

	protected override IEnumerator CoAttackCheck()
	{
		float time = GetAnimationTime(m_cAttack.m_AttackInfo.m_sAnimName, 0.6f);

		yield return new WaitForSeconds(time);

		if (m_cAttack.m_AttackInfo.m_iNextNum != 0)
		{
			// AI 와 Player를 나눔
			m_bNextAttack = true;
		}

		time = GetAnimationTime(m_cAttack.m_AttackInfo.m_sAnimName, 0.4f);

		yield return new WaitForSeconds(time);

		AttackEnd();
		yield break;
	}

	protected void ExcuteNextAttack(int id)
    {
		StopCoroutine(m_coAttackCheck);
		AttackEvent(id);
		m_bNextAttack = false;

	}
}
