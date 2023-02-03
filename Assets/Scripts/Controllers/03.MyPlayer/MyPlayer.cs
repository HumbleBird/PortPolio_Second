using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class MyPlayer : Player
{
	public Vector2 move;
	Option m_cOption = new Option();
	public GameObject m_FollwTarget = null;

	protected override void Init()
    {
        base.Init();
		SetKey();

		Managers.Object.MyPlayer = this;
	}

	protected override void UpdateController()
    {
        base.UpdateController();

		switch (eState)
		{
			case CreatureState.Idle:
				IdleAndMoveState();
				break;
			case CreatureState.Move:
				IdleAndMoveState();
				break;
		}

		//TODO temp
		m_cOption.InputOptionKey();
	}

	void IdleAndMoveState()
    {
		GetMoveInput();
		GetInputAttack();
		m_strAttack.InputMaintainKey();
		m_strAttack.InputOnekey();
	}

	bool m_bMoveInput = false;
	void GetMoveInput()
    {
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
			Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
		{
			m_bMoveInput = true;
		}
		else
			m_bMoveInput = false;
	}

	protected override void UpdateIdle()
    {
        base.UpdateIdle();

		if (m_bMoveInput == true)
        {
			if (m_bWaiting)
				return;

			eState = CreatureState.Move;
			return;
		}
	}

    // 걷기, 달리기 등
    protected override void UpdateMove()
    {
		if (m_bWaiting)
			return;

		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");

		move = new Vector3(horizontal, 0, vertical);

		if (Input.GetKey(KeyCode.LeftShift))
			SetMoveState(MoveState.Run);
		else if (eMoveState != MoveState.Crouch)
			SetMoveState(MoveState.Walk);

		Camera camera = Managers.Camera.m_Camera;

		Vector3 forward = camera.transform.forward;
		Vector3 right = camera.transform.right;
		forward.y = 0;
		right.y = 0;
		forward = forward.normalized;
		right = right.normalized;

		Vector3 forwardRelativeVerticalInput = vertical * forward;
		Vector3 rightRelativeHorizontalInput = horizontal * right;
		Vector3 CameraRelativeMovement = forwardRelativeVerticalInput + rightRelativeHorizontalInput;
		transform.Translate(CameraRelativeMovement, Space.World);


		if (m_bMoveInput == false && eMoveState != MoveState.Crouch)
			eState = CreatureState.Idle;
	}

	public void SetKey()
    {
		m_strAttack.SetKey();
		m_cOption.SetKey();
	}

	
}

