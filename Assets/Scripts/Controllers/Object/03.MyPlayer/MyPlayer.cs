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

	Dictionary<KeyCode, Action> keyDictionary;

	protected override void Start()
	{
		base.Start();

		m_tCamera = Camera.main;

		SetKeyMehod();
	}

	void SetKeyMehod()
    {
		keyDictionary = new Dictionary<KeyCode, Action>
		{
			// 여기는 한 번만 누르면 되는 것들
			// 스킬이나 팝업?
            //{ Managers.InputKey._binding.Bindings[UserAction.Jump],
			//  m_stPlayerMove.Jump()},
			
			//{ KeyCode.B, KeyDown_B },
			//{ KeyCode.C, KeyDown_C }
		};
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

    protected override void Update()
    {
        base.Update();

		if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.UI_Setting]))
			m_stPlayerMove.ShowInputKeySetting();
	}

    void GetInputKey()
    {
		GetDirInput();
		GetInputkeyAttack();
		SpecialAction();
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

	void GetDirInput()
	{
        if (Input.anyKeyDown)
        {
            foreach (var dic in keyDictionary)
            {
                if (Input.GetKeyDown(dic.Key))
                {
                    dic.Value();
                }
				else if (Input.GetKey(dic.Key))
				{
					dic.Value();
				}
				else if (Input.GetKeyUp(dic.Key))
				{
					dic.Value();
				}
			}
        }

        _moveKeyPressed = true;
		if (Input.GetKey(KeyCode.W) ||
		   Input.GetKey(KeyCode.A) ||
		   Input.GetKey(KeyCode.S) ||
		   Input.GetKey(KeyCode.D))
			State = CreatureState.Move;
		_moveKeyPressed = false;

	}

	protected void SpecialAction()
	{
		if(Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.Jump]))
			m_stPlayerMove.Jump();

		SpecialKey(UserAction.Sheild, "Shield");

		// 앉기
		if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.Crounch]))
			m_stPlayerMove.PlayerActionMove("Crounch", AnimationBlendState.Start);
		else if (Input.GetKey(Managers.InputKey._binding.Bindings[UserAction.Crounch]))
        {
			m_stPlayerMove.PlayerActionMove("Crounch", AnimationBlendState.Idle);

			// 쉴드
			if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.Sheild]))
				m_stPlayerMove.PlayerActionMove("Crounch Shield", AnimationBlendState.Start);
			else if (Input.GetKey(Managers.InputKey._binding.Bindings[UserAction.Sheild]))
				m_stPlayerMove.PlayerActionMove("Crounch Shield", AnimationBlendState.Idle);
			else if (Input.GetKeyUp(Managers.InputKey._binding.Bindings[UserAction.Sheild]))
				m_stPlayerMove.PlayerActionMove("Crounch Shield", AnimationBlendState.End);
		}
		else if (Input.GetKeyUp(Managers.InputKey._binding.Bindings[UserAction.Crounch]))
        {
			m_stPlayerMove.PlayerActionMove("Crounch", AnimationBlendState.End);
			m_stPlayerMove.PlayerActionMove("Crounch Shield", AnimationBlendState.End);
		}
	}

	void SpecialKey(UserAction action, string action2)
    {
		// 쉴드 ( 쉴드 캐릭터에게만 한정 되는데..)
		if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[action]))
			m_stPlayerMove.PlayerActionMove(action2, AnimationBlendState.Start);
		else if (Input.GetKey(Managers.InputKey._binding.Bindings[action]))
			m_stPlayerMove.PlayerActionMove(action2, AnimationBlendState.Idle);
		else if (Input.GetKeyUp(Managers.InputKey._binding.Bindings[action]))
			m_stPlayerMove.PlayerActionMove(action2, AnimationBlendState.End);
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
