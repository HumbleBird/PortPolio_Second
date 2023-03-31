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
	public float m_fRotationSpeed = 10f;

	float mouseX;
	float mouseY;
	float vertical;
	float horizontal;

	protected override void Init()
	{
		Managers.Object.myPlayer = this;

		base.Init();

		SetKey();
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

		InputOptionKey();

		if (Input.GetKeyDown(KeyCode.P)) //������ Q
		{
			Managers.Camera.HandleLockOn();
		}

		InputHandler();
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

	void InputHandler()
    {
		vertical = Input.GetAxis("Vertical");
		horizontal = Input.GetAxis("Horizontal");

		mouseX = Input.GetAxis("Mouse X");
		mouseY = Input.GetAxis("Mouse Y");

		m_MovementDirection = new Vector3(horizontal, 0, vertical);
	}

	void IdleAndMoveState()
    {
		if (m_bWaiting)
			return;

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



    // �ȱ�, �޸��� ��
    protected override void UpdateMove()
    {
		if (m_bWaiting)
			return;

		// ī�޶� ���� ĳ���� �̵� ���� ����
		Camera camera = Managers.Camera.m_Camera;
		m_MovementDirection = Quaternion.AngleAxis(camera.transform.rotation.eulerAngles.y, Vector3.up) * m_MovementDirection;

		// �տ� ��ֹ��� �ִٸ� �������� ���ϰ�
		if (Physics.Raycast(transform.position, transform.forward, 0.4f))
		{
			m_MovementDirection = Vector3.zero;
		}

		// �̵� ���� ���� : �ȱ�, �ٱ�
		if (Input.GetKey(KeyCode.LeftShift))
			SetMoveState(MoveState.Run);
		else if (Input.GetKeyUp(KeyCode.LeftShift))
			SetMoveState(MoveState.Walk);

		// �̵� �� ȸ��
		if (m_MovementDirection != Vector3.zero)
        {
			{
				transform.position += Time.deltaTime * m_Stat.m_fMoveSpeed * m_MovementDirection;
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_MovementDirection), m_fRotationSpeed * Time.deltaTime);
			}
		}

		if (m_MovementDirection == Vector3.zero)
        {
			eState = CreatureState.Idle;
		}
	}
}

