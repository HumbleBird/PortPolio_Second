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
		if (m_bCanAttack == true)
        {
			// 근접
			if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.BasicAttack]))
				AttackEvent(m_iBasicAttackNum);
			else if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.StrongAttack]))
				AttackEvent(m_iStrongAttackNum);
			else if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.Kick]))
				AttackEvent(m_iKickNum);

			// 원거리
			// 조준
			if(Input.GetMouseButton(2))
            {
				bool AttackRedey = false;

				// 조준 애니메이션
				//AttackRedey() -> AttackRedey = true;

				// 시위 겨누기 1번 -> 완료되면 AttackRedey = true
				// 시위 겨누는 중 무한 반복

				// 공격 준비가 완료가 되었다면
				if (AttackRedey)
                {
					// 마우스 왼쪽 클릭 발사
					if (Input.GetMouseButtonUp(0))
					{
						AttackEvent(FireArrow - m_iBasicAttackNum);
						AttackRedey = false;
					}
					// 조준 취소
					else if (Input.GetMouseButtonUp(2))
                    {
						CancelAttack();
						AttackRedey = false;
					}
				}
            }
		}
	}

    protected override void UpdateSkill() 
	{
		base.UpdateSkill();

		if (m_bNextAttack == true)
		{
			if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.BasicAttack]))
            {
				StopCoroutine(m_coAttackCheck);
				AttackEvent(m_strAttack.info.m_iNextNum);
				return;
			}
			else if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.StrongAttack]))
            {
				StopCoroutine(m_coAttackCheck);
				AttackEvent(m_strAttack.info.m_iNextNum);
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
