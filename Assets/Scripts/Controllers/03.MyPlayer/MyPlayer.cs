using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public partial class MyPlayer : Player
{
	public Vector3 m_MovementDirection;
	public GameObject m_FollwTarget = null;
	public float m_fRotationSpeed = 10f;

	protected override void Init()
	{
		base.Init();


		SetKey();

		m_FollwTarget = Managers.Resource.Instantiate("Objects/Camera/FollwTarget", transform);
	}

    protected override void SetInfo()
    {
        base.SetInfo();

		Managers.Object.myPlayer = this;
		Managers.Camera.Init();
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

		InputOptionKey();
	}

	void IdleAndMoveState()
    {
		GetMoveInput();
		
		if (m_bWaiting == false)
        {
			GetInputAttack();

			InputMaintainKey();
			InputOnekey();
		}
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

    // �ȱ�, �޸��� ��
    protected override void UpdateMove()
    {
		if (m_bWaiting)
			return;

		// �̵� �Է�
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");

		m_MovementDirection = new Vector3(horizontal, 0, vertical);

		// �̵� ���� ���� : �ȱ�, �ٱ�
		if (Input.GetKey(KeyCode.LeftShift))
			SetMoveState(MoveState.Run);
		else if (eMoveState != MoveState.Crouch)
			SetMoveState(MoveState.Walk);

		// ī�޶� ���� ĳ���� �̵� ���� ����
		Camera camera = Managers.Camera.m_Camera;
		m_MovementDirection = Quaternion.AngleAxis(camera.transform.rotation.eulerAngles.y, Vector3.up) * m_MovementDirection;

		// �̵� �� ȸ��
		if (m_MovementDirection != Vector3.zero)
        {
			{
				transform.position += Time.deltaTime * m_strStat.m_fMoveSpeed * m_MovementDirection;
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_MovementDirection), m_fRotationSpeed * Time.deltaTime);
			}
		}

		if (m_bMoveInput == false && eMoveState != MoveState.Crouch)
			eState = CreatureState.Idle;
	}
}

