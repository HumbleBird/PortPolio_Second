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
			// 근접
			if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.NormalAction]))
            {
				Managers.Battle.EventDelegateAttack += () => { StartCoroutine(m_cAttack.NormalAttack()); };
				AttackEvent(m_iBasicAttackNum);
            }
		}

		//if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.SpecialAction]))
		//{
		//	Managers.Battle.EventDelegateAttack += () => { StartCoroutine(m_cAttack.SpeacialAction()); };
		//	AttackEvent(m_iStrongAttackNum);
		//}
	}

    protected override void UpdateSkill() 
	{
		base.UpdateSkill();

		if (m_bNextAttack == true && m_strStat.m_fStemina != 0)
		{
			// 일반 공격
			if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.NormalAction]))
            {
				Managers.Battle.EventDelegateAttack += () => { StartCoroutine(m_cAttack.NormalAttack()); };
				ExcuteNextAttack(m_cAttack.m_AttackInfo.m_iNextNum);
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
