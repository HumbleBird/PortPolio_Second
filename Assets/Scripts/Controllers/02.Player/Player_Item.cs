using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

public partial class Player : Character
{
    public ItemSoket m_leftHandSlot { get; private set; }
    public ItemSoket m_RightHandSlot { get; private set; }

    public Weapon m_LeftWeapon; // 현재 장착 아이템들 중 들고 있는 왼손 무기
    public Weapon m_RightWeapon; // 현재 장착 아이템들 중 들고 있는 오른손 무기
    public Weapon m_UnarmedWeapon; // 퀵 슬롯 중 빈 칸

    public Weapon[] m_WeaponInRightHandSlots = new Weapon[2]; // 오른손 장비 장착 가능한 슬롯
    public Weapon[] m_WeaponInLeftHandSlots = new Weapon[2]; // 왼손 장비 장착 가능한 슬롯

    public int m_iCurrentRightWeaponIndex = -1;
    public int m_iCurrentLeftWeaponIndex = -1;

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
        if (isLeft)
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
        if (isLeft)
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

    public void ChangeRightWeapon()
    {
        m_iCurrentRightWeaponIndex = m_iCurrentRightWeaponIndex + 1;

        if (m_iCurrentRightWeaponIndex == 0 && m_WeaponInRightHandSlots[0] != null)
        {
            m_RightWeapon = m_WeaponInRightHandSlots[m_iCurrentRightWeaponIndex];
            LoadWeaponOnSlot(m_WeaponInRightHandSlots[m_iCurrentRightWeaponIndex], false);
        }
        else if (m_iCurrentRightWeaponIndex == 0 && m_WeaponInRightHandSlots[0] == null)
        {
            m_iCurrentRightWeaponIndex++;
        }
        else if (m_iCurrentRightWeaponIndex == 1 && m_WeaponInRightHandSlots[1] != null)
        {
            m_RightWeapon = m_WeaponInRightHandSlots[m_iCurrentRightWeaponIndex];
            LoadWeaponOnSlot(m_WeaponInRightHandSlots[m_iCurrentRightWeaponIndex], false);
        }
        else
        {
            m_iCurrentRightWeaponIndex++;
        }

        if (m_iCurrentRightWeaponIndex > m_WeaponInRightHandSlots.Length - 1)
        {
            m_iCurrentRightWeaponIndex = -1;
            m_RightWeapon = m_UnarmedWeapon;
            LoadWeaponOnSlot(m_UnarmedWeapon, false);
        }

        Managers.UIBattle.UIGameScene.UIPlayerInfo.RefreshItem();
    }

    public void ChangeLeftWeapon()
    {
        m_iCurrentLeftWeaponIndex = m_iCurrentLeftWeaponIndex + 1;

        if (m_iCurrentLeftWeaponIndex == 0 && m_WeaponInLeftHandSlots[0] != null)
        {
            m_LeftWeapon = m_WeaponInLeftHandSlots[m_iCurrentLeftWeaponIndex];
            LoadWeaponOnSlot(m_WeaponInLeftHandSlots[m_iCurrentLeftWeaponIndex], true);
        }
        else if (m_iCurrentLeftWeaponIndex == 0 && m_WeaponInLeftHandSlots[0] == null)
        {
            m_iCurrentLeftWeaponIndex++;
        }
        else if (m_iCurrentLeftWeaponIndex == 1 && m_WeaponInLeftHandSlots[1] != null)
        {
            m_LeftWeapon = m_WeaponInLeftHandSlots[m_iCurrentLeftWeaponIndex];
            LoadWeaponOnSlot(m_WeaponInLeftHandSlots[m_iCurrentLeftWeaponIndex], true);
        }
        else
        {
            m_iCurrentLeftWeaponIndex++;
        }

        if (m_iCurrentLeftWeaponIndex > m_WeaponInLeftHandSlots.Length - 1)
        {
            m_iCurrentLeftWeaponIndex = -1;
            m_LeftWeapon = m_UnarmedWeapon;
            LoadWeaponOnSlot(m_UnarmedWeapon, true);
        }

        Managers.UIBattle.UIGameScene.UIPlayerInfo.RefreshItem();
    }
    #endregion
}
