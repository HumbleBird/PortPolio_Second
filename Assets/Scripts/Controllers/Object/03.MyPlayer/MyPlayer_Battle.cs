using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class MyPlayer : Player
{
	//int m_iStrongAttackNum = 4;
	int m_iKickNum = 51;
	void GetInputkeyAttack()
	{
		if (_isNextCanAttack == false)
			return;

		if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.BasicAttack]))
		{
			eState = CreatureState.Skill;
		}
		else if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.StrongAttack]))
		{
			// ���� ����
			//m_strAttack.AttackInfoCal(m_iBasicAttackNum);
			// ���� �ִϸ��̼�
			eState = CreatureState.Skill;
		}
		else if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.Kick]))
		{
			m_strAttack.AttackInfoCal(m_iKickNum);
		}
	}
}
