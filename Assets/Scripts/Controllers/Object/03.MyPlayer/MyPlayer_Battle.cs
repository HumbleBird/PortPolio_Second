using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class MyPlayer : Player
{
	void GetInputAttack()
	{
		if (m_bCanAttack == false)
			return;

		if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.BasicAttack]))
			AttackEvent(m_strAttack.m_iBasicAttackNum);
		else if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.StrongAttack]))
			AttackEvent(m_strAttack.m_iStrongAttackNum);
		else if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.Kick]))
			AttackEvent(m_strAttack.m_iKickNum);
	}

	protected override IEnumerator CheckNextAttack()
	{
		AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo((int)Layers.BaseLayer);

		if (stateInfo.IsName(m_strAttack.info.m_sAnimName))
		{
			if (stateInfo.normalizedTime >= 0.6 && m_strAttack.info.m_iNextNum != 0)
			{
				// 어떤 공격 키를 눌렀는지에 따라서
				if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.BasicAttack])
					|| Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.StrongAttack]))
					AttackEvent(m_strAttack.info.m_iNextNum);
			}
		}

		yield return null;
	}
}
