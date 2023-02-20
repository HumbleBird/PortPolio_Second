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
	
	protected override void SetHp(int NewHp, GameObject attacker)
	{
		base.SetHp(NewHp, attacker);

		Managers.UIBattle.StatRefershUI();
		StartCoroutine(Managers.UIBattle.UIGameScene.UIPlayerInfo.DownHP());
	}

	protected override void SetStemina(float NewSetStamina)
	{
		base.SetStemina(NewSetStamina);

		Managers.UIBattle.StatRefershUI();
	}
}
