using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static Define;

// Input Handler
// MyPlayer UI
public partial class MyPlayer : Player
{
	float mouseX;
	float mouseY;

	Dictionary<KeyCode, Action> OptionKeyDic; // �ܹ߼�

    private void Awake()
    {
		Managers.Object.myPlayer = this;
	}

	protected override void Init()
	{
		base.Init();

		OptionKeyDic = new Dictionary<KeyCode, Action>
		{
			{ Managers.InputKey._binding.Bindings[UserAction.OpenMenu], () => {    Managers.UIBattle.ShowAndCloseUI<UI_SettingKey>(); }},
			{ KeyCode.I, () => {  Managers.UIBattle.ShowAndCloseUI<UI_Inven>(); }},
			{ KeyCode.O, () => {  Managers.UIBattle.ShowAndCloseUI<UI_Equipment>(); }},
		};
	}

    private void FixedUpdate()
    {
		float delta = Time.fixedDeltaTime;
		CameraController cc = Managers.Camera.m_Camera.GetComponentInParent<CameraController>();

		if (cc != null)
        {
			cc.FollwTarget(delta);
			cc.HandleCameraRotation(delta, mouseX, mouseY);
		}
    }

    protected override void Update()
    {
        base.Update();
		InputHandler();
	}

	void HandleMoveInput()
    {
		m_fVertical = Input.GetAxis("Vertical");
		m_fHorizontal = Input.GetAxis("Horizontal");

		mouseX = Input.GetAxis("Mouse X");
		mouseY = Input.GetAxis("Mouse Y");

		m_fMoveAmount = Mathf.Clamp01(Mathf.Abs(m_fHorizontal) + Mathf.Abs(m_fVertical));

		m_MovementDirection = new Vector3(m_fHorizontal, 0, m_fVertical);
	}

	void InputHandler()
    {
		// Input Move 
		HandleMoveInput();

		// Move State Set
		if (eState == CreatureState.Move)
        {
			// ���콺 Ű�е��϶�
			// Lock On Targeting ���� ���� ��
			// Walk - Left Alt + Move Key
			// Run - Move Key
			// Sprint - Space Key ������鼭 Move Key

			// ������� �������� ó���Ѵ�.
			// Walk - Left Alt + Move Key
			// Run - Move Key
			// Sprint - Shift + Move Key

			// �̵� ���� ���� �뽬/�ȱ�/�ٱ�
			if (Input.GetKey(KeyCode.LeftShift)) 
				SetMoveState(MoveState.Sprint);
			else if (Input.GetKey(KeyCode.LeftAlt))
				SetMoveState(MoveState.Walk);
			else
				SetMoveState(MoveState.Run); 

			// ����
			if (Input.GetKeyDown(KeyCode.V))
				HandleJumping();

			// ������ �� �齺��
			if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.Dash_BackStep_Roll]))
				RollAndBackStep();
		}

		if(eState != CreatureState.Dead)
        {
			// Attack
			if (m_Stat.m_fStemina != 0 && m_cAttack != null)
			{
				if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.Attack_RightHand]))
                {
					if (canDoCombo)
					{
						m_bComboFlag = true;
						HandleWeaponCombo(m_RightWeapon);
						m_bComboFlag = false;
					}
					else
                    {
						HandleLightAttack(m_RightWeapon);
					}
				}
				else if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.Attack_LeftHand]))
				{
					HandleHeavyAttack(m_RightWeapon);
				}
			}
		}

		// Option
		InputOptionKey();
	}

	public void InputOptionKey()
	{
		if (Input.anyKeyDown)
		{
			foreach (var dic in OptionKeyDic)
			{
				if (Input.GetKeyDown(dic.Key))
				{
					dic.Value();

					// Sound
					SoundPlay("UI On Off Sound");
				}
			}
		}
	}

	protected override void SetHp(int NewHp)
	{
		base.SetHp(NewHp);

		Managers.UIBattle.UIGameScene.UIPlayerInfo.RefreshUI();
		StartCoroutine(Managers.UIBattle.UIGameScene.UIPlayerInfo.DownHP());
    }

    protected override void SetStemina(float NewSetStamina)
    {
        base.SetStemina(NewSetStamina);

        Managers.UIBattle.UIGameScene.UIPlayerInfo.RefreshUI();
    }

	public void PlayerCanMove(bool can = true)
	{
		if (can)
		{
			m_bWaiting = false;

			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else
		{
			m_bWaiting = true;
			eState = CreatureState.Idle;

			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}


}

