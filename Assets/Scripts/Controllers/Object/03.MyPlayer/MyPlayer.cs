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
		m_stPlayerAction.SetKeyMehod();
	}

    protected override void UpdateController()
    {
        base.UpdateController();

		switch (State)
		{
			case CreatureState.Idle:
				GetInputKey();
				break;
			case CreatureState.Move:
				GetInputKey();
				break;
			case CreatureState.Skill:
				break;
			case CreatureState.Dead:
				break;
		}
	}

    void GetInputKey()
    {
		GetDirInput();
		GetInputkeyAttack();
	}

    bool _moveKeyPressed = false;
	protected override void UpdateIdle()
    {
		// 이동 상태로 갈지 확인
		if (_moveKeyPressed)
		{
			State = CreatureState.Move;
			return;
		}
	}

    protected override void Update()
    {
        base.Update();

		m_stPlayerAction.InputKey();
    }

    void GetDirInput()
	{
        _moveKeyPressed = true;
		if (Input.GetKey(KeyCode.W) ||
		   Input.GetKey(KeyCode.A) ||
		   Input.GetKey(KeyCode.S) ||
		   Input.GetKey(KeyCode.D))
			State = CreatureState.Move;
		_moveKeyPressed = false;

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

		float inputMagnitude = Mathf.Clamp01(move.magnitude);
		inputMagnitude /= 2;

		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			inputMagnitude *= 2;
			MoveSpeed = RunSpeed;
		}

		transform.position += move * MoveSpeed * Time.deltaTime;

		if (move != Vector3.zero)
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), 10 * Time.deltaTime);

		Animator.SetFloat("Sprint", inputMagnitude, 0.05f, Time.deltaTime);
	}

	public void Step()
    {

    }
}
