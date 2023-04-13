using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Player : Character
{
	protected bool m_bNextAttack = false;
    public UI_Interact UIInteract = null;
    public UI_Interact UIInteractPost = null;

    public override void AttackEvent(int id)
    {
        base.AttackEvent(id);

		eState = CreatureState.Skill;

		// 스테미너 감소
		float newStemina = m_Stat.m_fStemina - 10;
		SetStemina(newStemina);
	}

    public void RefreshAdditionalStat()
    {
        m_fWeaponDamage = 0;
        m_fArmorDefence = 0;

        foreach (Item item in Managers.Inventory.m_Items)
        {
            if (item.m_bEquipped == false)
                continue;

            switch (item.eItemType)
            {
                case ItemType.Weapon:
                    m_fWeaponDamage += ((Weapon)item).Damage;
                    break;
                case ItemType.Armor:
                    m_fArmorDefence += ((Armor)item).PhysicalResitance;
                    break;
                case ItemType.Consumable:
                    break;
                default:
                    break;
            }
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
                PlayAnimation("Falling To Landing");
                eMoveState = MoveState.None;
                float time = GetAnimationTime("Falling To Landing");
                StartCoroutine(WaitToState(time, CreatureState.Idle));
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
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeRightWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeLeftWeapon();
        }
    }

    public void HandleJumping()
    {
        if (m_bWaiting)
            return;

        // 점프는 달리는 도중에 눌러줘야합니다
        // 스페이스를 꾹 눌러서 달리는 도중에 다시 바로 스페이스를 눌러주면 점프해요.
        if (m_MovementDirection != Vector3.zero)
        {
            m_MovementDirection = m_Camera.transform.forward * vertical;
            m_MovementDirection += m_Camera.transform.forward * horizontal;
            PlayAnimation("Jump");
            m_MovementDirection.y = 0;
            Quaternion jumpRotation = Quaternion.LookRotation(m_MovementDirection);
            transform.rotation = jumpRotation;

        }
    }

    void StaminaGraduallyFillingUp()
    {
        float statValue = 0f;
        if (eState == CreatureState.Idle)
            statValue = 0.2f;
        else if (eState == CreatureState.Move)
        {
            if (eMoveState == MoveState.Walk)
                statValue = 0.15f;
            else if (eMoveState == MoveState.Run)
                statValue = -0.01f;
        }

        float newStemina = m_Stat.m_fStemina + statValue;

        SetStemina(newStemina);
    }

    public void CheckInteractableObject()
    {
        RaycastHit hit;

        Debug.DrawRay(transform.position + Vector3.up, transform.forward, Color.red, 1f);
        if(Physics.SphereCast(transform.position + Vector3.up, 0.3f, transform.forward, out hit, 1f, 1 << 13))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                if (UIInteract == null)
                {
                    UIInteract = Managers.UI.ShowPopupUI<UI_Interact>();
                    UIInteract.m_InteratableText.text = interactable.interactableText;
                }

                // 상호작용
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Managers.UI.ClosePopupUI();
                    interactable.Interact(this);
                    UIInteract = null;
                }
            }
        }
        else
        {
            // 상호작용 오브젝트와의 처음 접촉 시 뜨는 팝업 창이 거리가 멀어지면 닫음
            if (UIInteract != null)
            {
                Managers.UI.ClosePopupUI();
                UIInteract = null;
            }

            // 상호작용 오브젝트와 상호작용 후 뜨는 후속 팝업창을 수동으로 꺼줌
            if (UIInteractPost != null && Input.GetKeyDown(KeyCode.E))
            {
                Managers.UI.ClosePopupUI();
                UIInteractPost = null;
            }
        }
    }

    #region PlayerAction
    public void RollAndBackStep()
    {
        string animName = null;
        if (eState == CreatureState.Idle)
        {
            animName = "BackStep";
        }
        else if (eState == CreatureState.Move)
        {
            animName = "Run To Roll";
            eActionState = ActionState.Invincible;
        }

        PlayAnimation(animName);
        float time = GetAnimationTime(animName);
        Stop(time * 0.8f);
    }
    #endregion
}
