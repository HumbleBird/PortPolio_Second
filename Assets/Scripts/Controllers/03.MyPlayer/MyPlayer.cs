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
	float vertical;
	float horizontal;

	Dictionary<KeyCode, Action> OptionKeyDic; // 단발성

	protected override void Init()
	{
		Managers.Object.myPlayer = this;

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

		// 카메라를 향해 캐릭터 이동 방향 결정
		Camera camera = Managers.Camera.m_Camera;
		m_MovementDirection = Quaternion.AngleAxis(camera.transform.rotation.eulerAngles.y, Vector3.up) * m_MovementDirection;
	}

	protected override void UpdateIdle()
    {
        base.UpdateIdle();

		IdleAndMoveInput();
	}

    protected override void UpdateMove()
    {
		base.UpdateMove();

		IdleAndMoveInput();

		// 이동 상태 결정 : 걷기, 뛰기
		if (Input.GetKey(KeyCode.LeftShift))
			SetMoveState(MoveState.Run);
		else if (Input.GetKeyUp(KeyCode.LeftShift))
			SetMoveState(MoveState.Walk);
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

	void IdleAndMoveInput()
	{
		if (m_bWaiting)
			return;

		//GetInputAttack
		if (m_bCanAttack == true && m_Stat.m_fStemina != 0)
		{
			// 근접
			if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.NormalAction]))
			{
				AttackEvent(1);
			}
		}

		//GetKeyAction
		if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.Roll]))
			Roll();

		// Attack And Special Action
		if (Input.GetMouseButton(1))
			StartCoroutine(m_cAttack.SpeacialAction());
		else if (Input.GetMouseButtonUp(1))
			StartCoroutine(m_cAttack.SpeacialActionEnd());
	}

	void InputHandler()
    {
		// Move
		vertical = Input.GetAxis("Vertical");
		horizontal = Input.GetAxis("Horizontal");

		mouseX = Input.GetAxis("Mouse X");
		mouseY = Input.GetAxis("Mouse Y");

		m_MovementDirection = new Vector3(horizontal, 0, vertical);

		// Option
		InputOptionKey();

		//GetInputAttack
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

