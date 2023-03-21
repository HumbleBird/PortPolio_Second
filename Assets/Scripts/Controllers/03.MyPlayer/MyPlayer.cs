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
		if (m_bWaiting)
			return;

		GetMoveInput();
		GetInputAttack();
		GetKeyAction();
	}

	void GetKeyAction()
    {
		if(Input.GetKeyDown( Managers.InputKey._binding.Bindings[UserAction.Roll]))
			StartCoroutine(Roll());

		if (Input.GetMouseButton(1))
			StartCoroutine(m_cAttack.SpeacialAction());
		else if (Input.GetMouseButtonUp(1))
			StartCoroutine(m_cAttack.SpeacialActionEnd());
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
        {
			m_bMoveInput = false;
		}
	}

	protected override void UpdateIdle()
    {
        base.UpdateIdle();

		if (m_bMoveInput == true)
        {
			if (m_bWaiting)
				return;

			SetMoveState(MoveState.Walk);
			eState = CreatureState.Move;
			return;
		}
	}

    // 걷기, 달리기 등
    protected override void UpdateMove()
    {
		if (m_bWaiting)
			return;

		// 이동 입력
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");

		m_MovementDirection = new Vector3(horizontal, 0, vertical);
		

		// 이동 상태 결정 : 걷기, 뛰기
		if (Input.GetKey(KeyCode.LeftShift))
			SetMoveState(MoveState.Run);
		else if (Input.GetKeyUp(KeyCode.LeftShift))
			SetMoveState(MoveState.Walk);

		// 카메라를 향해 캐릭터 이동 방향 결정
		Camera camera = Managers.Camera.m_Camera;
		m_MovementDirection = Quaternion.AngleAxis(camera.transform.rotation.eulerAngles.y, Vector3.up) * m_MovementDirection;

		// 이동 및 회전
		if (m_MovementDirection != Vector3.zero)
        {
			{
				transform.position += Time.deltaTime * m_strStat.m_fMoveSpeed * m_MovementDirection;
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_MovementDirection), m_fRotationSpeed * Time.deltaTime);
			}
		}

		if (m_bMoveInput == false)
			eState = CreatureState.Idle;
	}
}

