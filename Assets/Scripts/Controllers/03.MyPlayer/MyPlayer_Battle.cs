using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class MyPlayer : Player
{
	public int m_iBasicAttackNum = 1;
	public int m_iStrongAttackNum = 4;
	public int m_iKickNum = 501;

	protected override void AttackEvent(int id)
    {
        base.AttackEvent(id);

		Managers.Camera.m_CameraEffect.Shake(1);
	}

	void GetInputAttack()
	{
		if (m_bCanAttack == true && m_strStat.m_fStemina != 0)
        {
			// ±Ÿ¡¢
			if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.BasicAttack]))
            {
				Managers.Battle.EventDelegateAttack += m_strAttack.NormalAttack;
				AttackEvent(m_iBasicAttackNum);
            }
			else if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.StrongAttack]))
            {
				Managers.Battle.EventDelegateAttack += m_strAttack.NormalAttack;
				AttackEvent(m_iStrongAttackNum);
			}
		}
	}

    protected override void UpdateSkill() 
	{
		base.UpdateSkill();

		if (m_bNextAttack == true && m_strStat.m_fStemina != 0)
		{
			if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.BasicAttack]))
            {
				Managers.Battle.EventDelegateAttack += m_strAttack.NormalAttack;
				ExcuteNextAttack(m_strAttack.info.m_iNextNum);
				return;
			}
			else if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.StrongAttack]))
            {
				Managers.Battle.EventDelegateAttack += m_strAttack.NormalAttack;
				ExcuteNextAttack(m_strAttack.info.m_iNextNum);
				return;
			}
		}
	}


	protected override void SetHp(int NewHp, GameObject attacker)
	{
		base.SetHp(NewHp, attacker);

		Managers.UIBattle.UIGameScene.UIPlayerInfo.RefreshUI();
		StartCoroutine(Managers.UIBattle.UIGameScene.UIPlayerInfo.DownHP());
	}

	protected override void SetStemina(float NewSetStamina)
	{
		base.SetStemina(NewSetStamina);

		Managers.UIBattle.UIGameScene.UIPlayerInfo.RefreshUI();
	}
}
