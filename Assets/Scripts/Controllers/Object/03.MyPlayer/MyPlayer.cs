using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class MyPlayer : Player
{
	public GameObject followTransform;
	Option m_cOption = new Option();
	Camera m_tCamera;

	protected override void Init()
	{
		base.Init();

		m_tCamera = Camera.main;
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

		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		Vector3 move = new Vector3(horizontal, 0, vertical);
		move = Quaternion.AngleAxis(m_tCamera.transform.rotation.eulerAngles.y, Vector3.up) * move;

		if (Input.GetKey(KeyCode.LeftShift))
			SetMoveState(MoveState.Run);
		else if (eMoveState != MoveState.Crouch)
			SetMoveState(MoveState.Walk);

		transform.position += move * m_strStat.m_fMoveSpeed * Time.deltaTime;

		if (move != Vector3.zero)
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), 10 * Time.deltaTime);
		
		if(m_bMoveInput == false && eMoveState != MoveState.Crouch)
			eState = CreatureState.Idle;
	}

	public void SetKey()
    {
		m_strAttack.SetKey();
		m_cOption.SetKey();
	}
}
