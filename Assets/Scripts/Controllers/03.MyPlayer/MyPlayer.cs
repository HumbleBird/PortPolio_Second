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

	Dictionary<KeyCode, Action> OptionKeyDic; // 단발성

    private void Awake()
    {
		Managers.Object.myPlayer = this;
	}

	protected override void Init()
	{
		base.Init();

		OptionKeyDic = new Dictionary<KeyCode, Action>
		{
			{ Managers.InputKey._binding.Bindings[UserAction.UI_Setting], () => {    Managers.UIBattle.ShowAndCloseUI<UI_SettingKey>(); }},
			{ Managers.InputKey._binding.Bindings[UserAction.UI_Inventory], () => {  Managers.UIBattle.ShowAndCloseUI<UI_Inven>(); }},
			{ Managers.InputKey._binding.Bindings[UserAction.UI_Equipment], () => {  Managers.UIBattle.ShowAndCloseUI<UI_Equipment>(); }},
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

	protected override void UpdateSkill()
	{
		base.UpdateSkill();

		// 일반 공격
		if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.NormalAction]))
		{
			m_bNextAttack = true;
		}
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
			// 마우스 키패드일때
			// Lock On Targeting 하지 않을 시
			// Walk - Left Alt + Move Key
			// Run - Move Key
			// Sprint - Space Key 누르고면서 Move Key

			// 약식으로 다음으로 처리한다.
			// Walk - Left Alt + Move Key
			// Run - Move Key
			// Sprint - Shift + Move Key

			// 이동 상태 결정 대쉬/걷기/뛰기
			if (Input.GetKey(KeyCode.LeftShift)) 
				SetMoveState(MoveState.Sprint);
			else if (Input.GetKey(KeyCode.LeftAlt))
				SetMoveState(MoveState.Walk);
			else
				SetMoveState(MoveState.Run); 

			// 점프
			if (Input.GetKeyDown(KeyCode.V))
				HandleJumping();

			// 구르기 및 백스템
			if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.Roll]))
				RollAndBackStep();
		}

		// Attack
		if(eState == CreatureState.Move || eState == CreatureState.Idle)
        {
			if (m_bCanAttack == true && m_Stat.m_fStemina != 0 && m_cAttack != null)
			{
				if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.NormalAction]))
				{
					m_cAttack.NormalAction();
				}
				else if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.SpecialAction]))
				{
					m_cAttack.SpecialAction();
				}
			}
		}

		// Option
		InputOptionKey();

		// Lock On
		if (Input.GetKeyDown(KeyCode.P)) //원래는 Q
			Managers.Camera.HandleLockOn();
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

