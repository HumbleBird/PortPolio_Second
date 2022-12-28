using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class MyPlayer : Player
{
	public GameObject followTransform;
	Camera m_tCamera;

	protected override void Start()
	{
		base.Start();

		m_tCamera = Camera.main;
		m_strCharacterAction.SetKeyMehod();
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

		
	}

	void IdleAndMoveState()
    {
		GetInputkeyAttack();
		m_strCharacterAction.InputMaintainKey();
		m_strCharacterAction.InputOnekey();
		StaminaGraduallyFillingUp();
	}

	protected override void UpdateIdle()
	{
		base.UpdateIdle();

		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
		    Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
			eState = CreatureState.Move;
	}

	// 걷기, 달리기 등
	protected override void UpdateMove()
    {
		if (waiting)
			return;

		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		MoveSpeed = WalkSpeed;

		Vector3 move = new Vector3(horizontal, 0, vertical);
		move = Quaternion.AngleAxis(m_tCamera.transform.rotation.eulerAngles.y, Vector3.up) * move;

		sprint = Mathf.Clamp01(move.magnitude);
		sprint /= 2;

		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			ActionStateEnd();
			sprint *= 2;
			MoveSpeed = RunSpeed;
		}

		transform.position += move * MoveSpeed * Time.deltaTime;

		if (move != Vector3.zero)
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), 10 * Time.deltaTime);

		//Animator.SetFloat("Sprint", sprint, 0.05f, Time.deltaTime);
	}

	public void Step()
    {
		// TODO Effect Music
    }
}
