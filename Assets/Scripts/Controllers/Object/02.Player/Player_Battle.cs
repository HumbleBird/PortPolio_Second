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

	public override void AttackEnd()
	{
		base.AttackEnd();

		StopCoroutine(CheckNextAttack());
	}

	protected virtual IEnumerator CheckNextAttack()
	{
		AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo((int)Layers.BaseLayer);

		if (stateInfo.IsName(m_strAttack.info.m_sAnimName))
		{
			if (stateInfo.normalizedTime >= 0.6)
			{
				// 어떤 공격 키를 눌렀는지에 따라서
				if (m_bNextAttack == true)
					AttackEvent(m_strAttack.info.m_iNextNum);
			}
		}

		yield return null;
	}
}
