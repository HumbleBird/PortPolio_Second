﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Player : Character
{
	protected bool m_bNextAttack = false;

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

        Debug.DrawRay(transform.position, transform.forward, Color.red, 1f);
        if(Physics.SphereCast(transform.position + Vector3.up, 0.3f, transform.forward, out hit, 1f, 1 << 13))
        {
            // 필드 아이템
            if(hit.collider.tag == "InteractablePickItem")
            {
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                if(interactableObject != null)
                {
                    string interactableText = interactableObject.interactableText;

                    if(Input.GetKeyDown(KeyCode.E))
                    {
                        interactableObject.Interact(this);
                    }
                }
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
