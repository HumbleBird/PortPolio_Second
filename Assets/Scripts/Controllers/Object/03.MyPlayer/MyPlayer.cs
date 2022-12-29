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
				GetInputKey();
				StaminaGraduallyFillingUp();
				break;
			case CreatureState.Move:
				GetInputKey();
				StaminaGraduallyFillingUp();
				break;
		}
	}

	void GetInputKey()
    {
		GetDirInput();
		GetInputkeyAttack();
		m_strCharacterAction.InputMaintainKey();
		m_strCharacterAction.InputOnekey();
	}

	void GetDirInput()
    {
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
			Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
			eState = CreatureState.Move;
		else
			eState = CreatureState.Idle;
	}

	// �ȱ�, �޸��� ��
	protected override void UpdateMove()
    {
		if (waiting)
			return;

		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		Vector3 move = new Vector3(horizontal, 0, vertical);
		move = Quaternion.AngleAxis(m_tCamera.transform.rotation.eulerAngles.y, Vector3.up) * move;

<<<<<<< HEAD
		if (Input.GetKey(KeyCode.LeftShift))
		{
			ActionStateEnd();
			Sprint = RunSprint;
=======
		float inputMagnitude = Mathf.Clamp01(move.magnitude);
		inputMagnitude /= 2;

		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			ActionStateEnd();
			inputMagnitude *= 2;
>>>>>>> parent of 6f677e5 (애니메이션 재조정)
			MoveSpeed = RunSpeed;
		}
        else
        {
			Sprint = WalkSprint;
			MoveSpeed = WalkSpeed;
		}

		transform.position += move * MoveSpeed * Time.deltaTime;

		if (move != Vector3.zero)
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), 10 * Time.deltaTime);
<<<<<<< HEAD
=======

			Animator.SetFloat("Sprint", inputMagnitude, 0.05f, Time.deltaTime);
>>>>>>> parent of 6f677e5 (애니메이션 재조정)
	}

	public void Step()
    {
		// TODO Effect Music
    }
}
