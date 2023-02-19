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

	protected override IEnumerator CoAttackCheck()
	{
		yield return new WaitForSeconds(GetAnimationTime(m_strAttack.info.m_sAnimName, 0.6f));

		if (m_bNextAttack == true && m_strAttack.info.m_iNextNum != 0)
		{
			// TODO 공격 콤보
			if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.BasicAttack])
				|| Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.StrongAttack]))
				AttackEvent(m_strAttack.info.m_iNextNum);

			Debug.Log("다음 공격 가능");

			yield break;
		}

		yield return new WaitForSeconds(GetAnimationTime(m_strAttack.info.m_sAnimName, 0.4f));
		{
			AttackEnd();
			Debug.Log("공격 끝");

			yield break;
		}
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
