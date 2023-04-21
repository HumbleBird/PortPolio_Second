using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Player : Character
{
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
                statValue = 0.1f;
            else if (eMoveState == MoveState.Sprint)
                statValue = -0.1f;
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

    public void RollAndBackStep()
    {
        if (m_bWaiting)
            return;

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
}
