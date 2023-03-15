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
			// ����
			if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.BasicAttack]))
				AttackEvent(m_iBasicAttackNum);
			else if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.StrongAttack]))
				AttackEvent(m_iStrongAttackNum);
			else if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.Kick]))
				AttackEvent(m_iKickNum);

			// ���Ÿ�
			// ����
			if(Input.GetMouseButton(2))
            {
				bool AttackRedey = false;

				// ���� �ִϸ��̼�
				//AttackRedey() -> AttackRedey = true;

				// ���� �ܴ��� 1�� -> �Ϸ�Ǹ� AttackRedey = true
				// ���� �ܴ��� �� ���� �ݺ�

				// ���� �غ� �Ϸᰡ �Ǿ��ٸ�
				if (AttackRedey)
                {
					// ���콺 ���� Ŭ�� �߻�
					if (Input.GetMouseButtonUp(0))
					{
						AttackEvent(FireArrow - m_iBasicAttackNum);
						AttackRedey = false;
					}
					// ���� ���
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
