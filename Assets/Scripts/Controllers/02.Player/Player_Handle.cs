using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Player : Character
{
    private void HandleRotation()
    {
        if (m_bLockOnFlag)
        {
            if (m_bSprint || m_bWaiting == false)
            {
                Vector3 targetDirection = Vector3.zero;
                targetDirection = m_Camera.transform.forward * m_fVertical;
                targetDirection += m_Camera.transform.right * m_fHorizontal;
                targetDirection.Normalize();
                targetDirection.y = 0;

                if (targetDirection == Vector3.zero)
                    targetDirection = transform.forward;

                Quaternion tr = Quaternion.LookRotation(targetDirection);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, m_fRotationSpeed * Time.deltaTime);

                transform.rotation = targetRotation;
            }
            else
            {
                Vector3 rotationDirection = m_MovementDirection;
                rotationDirection = m_CameraController.m_trCurrentLockOnTarget.position - transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();

                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, m_fRotationSpeed * Time.deltaTime);

                transform.rotation = targetRotation;
            }
        }
        else
        {
            Vector3 targetDir = Vector3.zero;

            targetDir = m_Camera.transform.forward * m_fVertical;
            targetDir += m_Camera.transform.right * m_fHorizontal;

            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
                targetDir = transform.forward;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, m_fRotationSpeed * Time.deltaTime);

            transform.rotation = targetRotation;
        }

    }

    public void HandleFalling()
    {
        Vector3 origin = transform.position;
        origin.y += m_Collider.bounds.size.y / 2;

        RaycastHit hit;

        float fallingSpeed = 45f;

        float minimunDistanceNeededToBeginFall = origin.y - transform.position.y + 0.04f;

        // 앞에 장애물이 있다면 움직이지 못하게
        if (Physics.Raycast(transform.position, transform.forward, 0.4f))
        {
            m_MovementDirection = Vector3.zero;
        }

        if (eMoveState == MoveState.Falling)
        {
            m_Rigidbody.AddForce(-Vector3.up * fallingSpeed);
            m_Rigidbody.AddForce(m_MovementDirection * fallingSpeed / 10f);
        }

        Vector3 dir = m_MovementDirection;
        dir.Normalize();
        origin = origin + dir * -0.2f;

        Vector3 targetPotion = transform.position;

        Debug.DrawRay(origin, -Vector3.up * minimunDistanceNeededToBeginFall, Color.red, 0.1f, false);

        // 땅에 착지
        if (Physics.Raycast(origin, -Vector3.up, out hit, minimunDistanceNeededToBeginFall, 1 << (int)Layer.Obstacle))
        {

            Vector3 tp = hit.point;
            targetPotion.y = tp.y;

            if (eMoveState == MoveState.Falling)
            {
                Debug.Log("you are flying the time : " + m_FallingWatch.Elapsed.TotalSeconds);
                PlayAnimation("Falling To Landing", true);
                eMoveState = MoveState.None;
                m_FallingWatch.Stop();

                if (m_FallingWatch.Elapsed.TotalSeconds >= 0.5)
                {
                    if (m_FallingWatch.Elapsed.TotalSeconds >= 3)
                    {
                        Debug.Log("사망");
                        return;
                    }

                    HitEvent(this, (int)(m_FallingWatch.Elapsed.TotalSeconds * 10), false);
                }
            }

        }
        // 낙하 중
        else
        {
            if (eMoveState != MoveState.Falling)
            {
                Vector3 vel = m_Rigidbody.velocity;
                vel.Normalize();
                m_Rigidbody.velocity = vel * (10 / 2);

                m_FallingWatch.Reset();
                m_FallingWatch.Start();

                SetMoveState(MoveState.Falling);
            }
        }

        if (eMoveState != MoveState.Falling)
        {
            if (m_MovementDirection != Vector3.zero)
                transform.position = Vector3.Lerp(targetPotion, transform.position, Time.deltaTime);
            else
                transform.position = targetPotion;
        }
    }

    private void HandleQuickSlotsInput()
    {
        if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.SwitchRightHandWeapon]))
        {
            ChangeHandWeapon(false);
        }
        else if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.switchLeftHandWeapon]))
        {
            ChangeHandWeapon(true);
        }
    }

    public void HandleJumping()
    {
        if (m_bWaiting)
            return;

        // 점프는 달리는 도중에 눌러줘야합니다
        // 스페이스를 꾹 눌러서 달리는 도중에 다시 바로 스페이스를 눌러주면 점프해요.
        m_MovementDirection = m_Camera.transform.forward * m_fVertical;
        m_MovementDirection += m_Camera.transform.forward * m_fHorizontal;
        PlayAnimation("Jump");
        m_MovementDirection.y = 0;
        Quaternion jumpRotation = Quaternion.LookRotation(m_MovementDirection);
        transform.rotation = jumpRotation;
    }

    public void HandleLockOnInput()
    {
        if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.CameraReset_LockOn]) && !m_bLockOnFlag)
        {
            m_CameraController.HandleLockOn();
            if (m_CameraController.m_trNearestLockOnTarget != null)
            {
                m_CameraController.m_trCurrentLockOnTarget = m_CameraController.m_trNearestLockOnTarget;
                m_bLockOnFlag = true;
            }
        }
        else if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.CameraReset_LockOn]) && m_bLockOnFlag)
        {
            m_bLockOnFlag = false;
            m_CameraController.ClearLockOnTargets();
        }

        if (m_bLockOnFlag && Input.GetKeyDown(KeyCode.R)) // 좌측 키 Right_Stick_Left_Input
        {
            m_CameraController.HandleLockOn();
            if (m_CameraController.m_trleftLockTarget != null)
            {
                m_CameraController.m_trCurrentLockOnTarget = m_CameraController.m_trleftLockTarget;
            }
        }

        if (m_bLockOnFlag && Input.GetKeyDown(KeyCode.T)) // 우측 키 Right_Stick_Left_Input
        {
            m_CameraController.HandleLockOn();
            if (m_CameraController.m_trRightLockTarget != null)
            {
                m_CameraController.m_trCurrentLockOnTarget = m_CameraController.m_trRightLockTarget;
            }
        }

        m_CameraController.SetCameraHeight();
    }

    public void HandleWeaponCombo(Weapon weapon)
    {
        if(m_bComboFlag)
        {
            if (m_sLastAttack == weapon.m_sLight_Attack_1)
            {
                PlayAnimation(weapon.m_sLight_Attack_2);
            }
        }
    }

    // Attack
    public void HandleLightAttack(Weapon weapon)
    {
        string attackName = null;
        if (m_bTwoHandFlag)
        {
            attackName = weapon.m_sTwo_Hand_Idle;
        }
        else
        {
            attackName = weapon.m_sLight_Attack_1;
        }

        m_sLastAttack = attackName;

        // 애니메이션 실행
        PlayAnimation(attackName);

        eState = CreatureState.Skill;

        // 스테미너 감소
        float newStemina = m_Stat.m_fStemina - 10;
        SetStemina(newStemina);
    }

    public void HandleHeavyAttack(Weapon weapon)
    {
        string attackName = null;
        attackName = weapon.m_sHeavy_Attack_1;
        m_sLastAttack = attackName;

        // 애니메이션 실행
        PlayAnimation(attackName);

        eState = CreatureState.Skill;

        // 스테미너 감소
        float newStemina = m_Stat.m_fStemina - 10;
        SetStemina(newStemina);
    }

    // 오른손 무기를 중점으로 두 손 무기로 변형
    public void HandleTwoHandInput()
    {
        if (Input.GetKeyDown(Managers.InputKey._binding.Bindings[UserAction.TwoHandWeapon]))
        {
            m_bTwoHandFlag = !m_bTwoHandFlag;

            if (m_bTwoHandFlag)
            {
                // Enable Two Handing
                LoadWeaponOnSlot(m_RightWeapon, false);

            }
            else
            {
                // Disable Two Handing
                LoadWeaponOnSlot(m_RightWeapon, false);
                LoadWeaponOnSlot(m_LeftWeapon, true);

            }
        }

    }
}
