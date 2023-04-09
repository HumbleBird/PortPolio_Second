using System;
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

    #region Item
    public void EquipItem(Item equipItem)
    {
        if (equipItem == null)
            return;

        Item item = Managers.Inventory.Find(i =>
        i.Id == equipItem.Id &&
        i.InventorySlot == equipItem.InventorySlot);

        if (item == null)
            return;

        // 아이템 해제
        if (Managers.UIBattle.AreTheSlotsForThatItemFull(item) && item.m_bEquipped == false)
        {
            // TODO Item Switch
            Debug.Log("장비 창의 빈 칸이 없습니다. 장비 창의 아이템을 비워주세요.");
            return;
        }
        else
        {
            item.m_bEquipped = equipItem.m_bEquipped;

            if (item.eItemType == ItemType.Weapon)
            {
                LoadWeaponOnSlot((Weapon)item, false);
            }
        }

        RefreshAdditionalStat();
    }

    public void UnEquipItem(Item unEquipItem)
    {
        if (unEquipItem == null)
            return;

        // 해제하려는 아이템 인벤토리에서 리프레쉬
        Item item = Managers.Inventory.Find(i =>
                      i.m_bEquipped && i.InventorySlot == unEquipItem.InventorySlot &&
                      i.eItemType == unEquipItem.eItemType);

        item.m_bEquipped = false;
        UnLoadWeaponOnSlot(false);
    }

    // 소모품 아이템
    public void UseItem(Item item)
    {
        Debug.Log("아이템 사용");

        // TODO 아이템 효과에 따라
        // 대분류 : consumable (포션, 퀘스트 아이템 등)
        // 분류 : potionEffect (포션 종류에 따라)
        // 애니메이션, 사운드
    }

    public void BuyItem(Item item, int count)
    {
        // 조건
        if (item.m_iPrice * count > m_iHaveMoeny)
        {
            Debug.Log("돈이 부족합니다.");
            Managers.UI.ClosePopupUI();
            return;
        }

        if (Managers.Inventory.GetEmptySlot() == null)
        {
            Debug.Log("인벤토리에 빈 공간이 없습니다.");
            Managers.UI.ClosePopupUI();
            return;
        }

        // 구입 성공!
        m_iHaveMoeny -= item.m_iPrice * count;
        item.Count = count;
        Managers.Battle.AddItemtoPlayer(this, item);
        Managers.UI.ClosePopupUI();
        Managers.UIBattle.RefreshUI<UI_Shop>();
        Debug.Log("아이템 구입");

        // 아이템 정보를 받아옴
        // 가격은 아이템에서 뽑아오고
        // 수량은 정해진 한도 내에서
        // 구매하면 판매 수량 깍고, 플레이어 소지 돈 감소, 인벤
    }
    #endregion

    #region Weapon

    void WeaponInit()
    {
        ItemSoket[] weaponHolderSlots = GetComponentsInChildren<ItemSoket>();
        foreach (ItemSoket weaponslot in weaponHolderSlots)
        {
            if (weaponslot.isLeftHandSlot)
            {
                m_leftHandSlot = weaponslot;
            }
            else if (weaponslot.isRightHandSlot)
            {
                m_RightHandSlot = weaponslot;
            }
        }
    }

    public void LoadWeaponOnSlot(Weapon weaponItem, bool isLeft)
    {
        if(isLeft)
        {
            m_leftHandSlot.LoadWeaponModel(weaponItem);
        }
        else
        {
            m_RightHandSlot.LoadWeaponModel(weaponItem);
        }

        m_fWeaponDamage = weaponItem.Damage;
        m_Stat.m_iOriginalAtk = (int)m_fWeaponDamage;

        switch (weaponItem.eWeaponType)
        {
            case WeaponType.Daggers:
                m_cAttack = new Blow();
                m_cAttack.m_eWeaponType = WeaponType.Daggers;
                break;
            case WeaponType.StraightSwordsGreatswords:
                m_cAttack = new Blow();
                m_cAttack.m_eWeaponType = WeaponType.StraightSwordsGreatswords;
                break;
            case WeaponType.Shield:
                break;
        }

        m_cAttack.m_cGo = this;
    }

    public void UnLoadWeaponOnSlot(bool isLeft)
    {
        if(isLeft)
        {
            m_leftHandSlot.UnloadWeapon();
        }
        else
        {
            m_RightHandSlot.UnloadWeapon();
        }

        m_fWeaponDamage = 0;
        m_Stat.m_iOriginalAtk = 0;
        m_cAttack = null;
    }

    #endregion

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
