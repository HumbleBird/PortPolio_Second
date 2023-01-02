using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class MyPlayer : Player
{

	void GetInputAttack()
	{
		if (_isNextCanAttack == false)
			return;

		if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.BasicAttack]))
			m_strAttack.AttackInfoCal(m_iBasicAttackNum);
		else if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.StrongAttack]))
			m_strAttack.AttackInfoCal(m_iStrongAttackNum);
		else if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.Kick]))
			m_strAttack.AttackInfoCal(m_iKickNum);
	}
}
