using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class MyPlayer : Player
{
	void GetInputkeyAttack()
	{
		if (Input.GetMouseButtonDown(0) && _isNextCanAttack)
		{
			_isNextCanAttack = false;
			eState = CreatureState.Skill;
			m_strAttack.BasicAttack();
		}
	}
}
