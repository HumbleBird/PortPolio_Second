using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class MyPlayer : Player
{
	void GetInputkeyAttack()
	{
		if (m_fCoolTime <= 0)
		{
			if (Input.GetMouseButtonDown(0) && _isNextCanAttack)
			{
				_isNextCanAttack = false;
				State = CreatureState.Skill;
				_attack.BasicAttack();
			}
		}
		else
		{
			m_fCoolTime -= Time.deltaTime;
		}
	}
}
