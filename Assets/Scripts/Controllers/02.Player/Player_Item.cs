using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

public partial class Player : Character
{
    #region Item

    // 아이템 장착, 인벤토리에서 아이템(장비, 소모성 아이템, 장신구 등)을 클릭하여
    // 장비창에 장착 시킴.
    // 현재는 왼쪽의 빈칸부터 채워 넣음.
    public void EquipItem(Item equipItem)
    {
        if (equipItem == null)
            return;

        Item item = Managers.Inventory.Find(i =>
        i.Id == equipItem.Id &&
        i.InventorySlot == equipItem.InventorySlot);

        if (item == null)
            return;

        int? slot = Managers.Equipment.GetEmptySlot(item);
        if (slot == null)
            return;

        // TODO Item Switch

        // 아이템 해제
        if (Managers.UIBattle.AreTheSlotsForThatItemFull(item) && item.m_bEquipped == false)
        {
            Debug.Log("장비 창의 빈 칸이 없습니다. 장비 창의 아이템을 비워주세요.");
            return;
        }
        // 아이템 장착
        else
        {
            item.m_bEquipped = equipItem.m_bEquipped;

            Managers.Equipment.Add((int)slot, item);

            //if (item.eItemType == ItemType.Weapon)
            //{
            //    LoadWeaponOnSlot((Weapon)item, false);
            //}
        }

        RefreshAdditionalStat(); // TODO
        Managers.UIBattle.RefreshUI<UI_Equipment>();
    }

    // 장착 아이템 해제
    // 장착된 아이템을 장착 해제 시킨다.
    public void UnEquipItem(Item unEquipItem)
    {
        if (unEquipItem == null)
            return;

        Item item = Managers.Inventory.Find(i =>
                      i.m_bEquipped && i.InventorySlot == unEquipItem.InventorySlot &&
                      i.eItemType == unEquipItem.eItemType);

        item.m_bEquipped = false;
        UnLoadWeaponOnSlot(false);
    }

    // 아이템 사용
    // 소모성 아이템(포션, 투척 무기, 길 찾기 등)을 사용한다.
    public void UseItem(Item item)
    {
        Debug.Log("아이템 사용");

        // TODO 아이템 효과에 따라
        // 대분류 : consumable (포션, 퀘스트 아이템 등)
        // 분류 : potionEffect (포션 종류에 따라)
        // 애니메이션, 사운드
    }

    // 아이템 구매
    // 상점에서 소울(돈)을 지불하여 아이템을 구매한다.
    public void BuyItem(Item item, int count)
    {
        // 조건
        if (item.m_iPrice * count > m_iHaveMoeny)
        {
            Debug.Log("돈이 부족합니다.");
            Managers.UI.ClosePopupUI();
            return;
        }

        // TODO DELETE
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
    }
    #endregion

    #region Weapon

    // 플레이어의 양 손에 있는 슬롯 정보를 가져온다.
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
            else if (weaponslot.isBackSlot)
            {
                m_BackSlot = weaponslot;
            }
        }
    }

    // 아이템 로드
    // 아이템 정보를 받아 아이템 프리팹을 로드 시킨다.
    public void LoadWeaponOnSlot(Weapon weaponItem, bool isLeft)
    {
        if (isLeft)
        {
            m_leftHandSlot.CurrentWeapon = weaponItem;
            m_leftHandSlot.LoadWeaponModel(weaponItem);

            #region Handle Left Weapon Idle Animation
            if (weaponItem != null && weaponItem != m_UnarmedWeapon)
            {
                PlayAnimation(weaponItem.m_sLeft_Hand_Idle);
            }
            else
            {
                PlayAnimation("Left Arm Empty");
            }
            #endregion
        }
        else
        {
            if(m_bTwoHandFlag)
            {
                m_BackSlot.LoadWeaponModel(m_leftHandSlot.CurrentWeapon);
                m_leftHandSlot.UnloadWeaponAndDestroy();
                PlayAnimation(weaponItem.m_sTwo_Hand_Idle);
            }
            else
            {
                #region Handle Right Weapon Idle Animation
                PlayAnimation("Both Arm Empty");

                m_BackSlot.UnloadWeaponAndDestroy();
                if (weaponItem != null && weaponItem != m_UnarmedWeapon)
                {
                    PlayAnimation(weaponItem.m_sRight_Hand_Idle);
                }
                else
                {
                    PlayAnimation("Right Arm Empty");
                }
                #endregion
            }

            m_RightHandSlot.CurrentWeapon = weaponItem;
            m_RightHandSlot.LoadWeaponModel(weaponItem);

        }
    }

    // 아이템 로드 해제
    // 현재 로드한 아이템을 슬롯에서 해제
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
    }

    // 아이템 체인지
    // 장비창에 저장되어 있는 아이템에 한해 아이템을 교체 시킨다.
    public void ChangeHandWeapon(bool isLeft)
    {
        int index = 0;
        Weapon[] weaponhandslots;

        if(isLeft)
        {
            index = m_iCurrentLeftWeaponIndex;
            weaponhandslots = Managers.Equipment.m_ListWeaponInLeftHandSlots;
        }
        else
        { 
            index = m_iCurrentRightWeaponIndex;
            weaponhandslots = Managers.Equipment.m_ListWeaponInRightHandSlots;
        }

        index++;

        // i번 칸의 아이템이 없을 경우 i + 1번칸 검사
        for (int i = 0; i < 3; i++)
        {
            if (index == i && weaponhandslots[i] != null)
            {
                if (isLeft)
                {
                    // 방패
                    m_LeftWeapon = weaponhandslots[index];
                }
                else
                {
                    // 무기
                    m_RightWeapon = weaponhandslots[index];

                    // 무기 타입에 따른 공격 모션 변경
                    SetAttackType(m_RightWeapon);

                    // 데미지 더해주기.
                    m_fWeaponDamage = m_RightWeapon.Damage;
                    m_Stat.m_iOriginalAtk = (int)m_fWeaponDamage;
                }

                LoadWeaponOnSlot(weaponhandslots[index], isLeft);

                break;
            }
            else if (index == i && weaponhandslots[i] == null)
            {
                index++;
            }
        }

        // 아무것도 들지 않음
        if (index > weaponhandslots.Length - 1)
        {
            index = -1;

            if (isLeft)
            {
                m_LeftWeapon = m_UnarmedWeapon;
            }
            else
            {
                m_RightWeapon = m_UnarmedWeapon;

                m_fWeaponDamage = 0;
                m_Stat.m_iOriginalAtk = 0;
                m_cAttack = null;
            }

            LoadWeaponOnSlot(m_UnarmedWeapon, false);
        }

        if (isLeft)
        {
            m_iCurrentLeftWeaponIndex = index;
            Managers.Equipment.m_ListWeaponInLeftHandSlots  = weaponhandslots;
        }
        else
        {
            m_iCurrentRightWeaponIndex = index;
            Managers.Equipment.m_ListWeaponInRightHandSlots = weaponhandslots;
        }

        // UI

        Managers.UIBattle.UIGameScene.UIPlayerInfo.RefreshItem();
    }

    // 장착한 아이템에 따른 공격 매커니즘 변화
    private void SetAttackType(Weapon weapon)
    {
        switch (weapon.eWeaponType)
        {
            case WeaponType.Daggers:
                m_cAttack = new Blow();
                break;
            case WeaponType.StraightSwordsGreatswords:
                m_cAttack = new Blow();
                break;
            case WeaponType.Greatswords:
                break;
            case WeaponType.UltraGreatswords:
                break;
            case WeaponType.CurvedSword:
                break;
            case WeaponType.Katanas:
                break;
            case WeaponType.CurvedGreatswords:
                break;
            case WeaponType.PiercingSwords:
                break;
            case WeaponType.Axes:
                break;
            case WeaponType.Greataxes:
                break;
            case WeaponType.Hammers:
                break;
            case WeaponType.GreatHammers:
                break;
            case WeaponType.FistAndClaws:
                break;
            case WeaponType.SpearsAndPikes:
                break;
            case WeaponType.Halberds:
                break;
            case WeaponType.Reapers:
                break;
            case WeaponType.Whips:
                break;
            case WeaponType.Bows:
                break;
            case WeaponType.Greatbows:
                break;
            case WeaponType.Crossbows:
                break;
            case WeaponType.Staves:
                break;
            case WeaponType.Flames:
                break;
            case WeaponType.Talismans:
                break;
            case WeaponType.SacredChimes:
                break;
            case WeaponType.Shield:
                break;
            default:
                break;
        }

        m_cAttack.m_eWeaponType = weapon.eWeaponType;
        m_cAttack.m_cGo = this;
    }
    #endregion
}
