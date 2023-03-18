using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Player : Character
{
    public int m_iWeaponDamage { get; private set; }
    public int m_iArmorDefence { get; private set; }
    public int m_iHaveMoeny { get; private set; }

    public override int m_TotalAttack { get { return m_strStat.m_iAtk + m_iWeaponDamage; } }
    public override int m_TotalDefence { get { return m_strStat.m_iDef + m_iArmorDefence; } }

    protected Coroutine cStaminaGraduallyFillingUp;

    protected override void Init()
    {
        base.Init();

        cStaminaGraduallyFillingUp = StartCoroutine(StaminaGraduallyFillingUp());
        m_iHaveMoeny = 10000;
    }

    protected override void SetInfo()
    {
        base.SetInfo();

        Table_Player.Info pinfo = Managers.Table.m_Player.Get(ID);

        if (pinfo == null)
        {
            Debug.LogError("해당하는 Id의 플레이어가 없습니다.");
            return;
        }

        m_strStat.m_tStatInfo = Managers.Table.m_Stat.Get(ID);
        eObjectType = ObjectType.Player;

        ChangeClass(pinfo.m_iClass);
    }

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
            Debug.Log("장비 창의 빈 칸이 없습니다. 장비 창의 아이템을 비워주세요.");
            return;
        }
        else
        {
            item.m_bEquipped = equipItem.m_bEquipped;

            Managers.UIBattle.RefreshUI<UI_Inven>();
        }


        RefreshAdditionalStat();
    }

    public void UnEquipItem(Item equipItem)
    {
        Item item = null;
        // 장착 해제
        if (equipItem.m_bEquipped)
        {
            Item unequipItem = null;

            if (item.eItemType == ItemType.Weapon)
            {
                unequipItem = Managers.Inventory.Find(i =>
                i.m_bEquipped && i.eItemType == ItemType.Weapon);
            }
            else if (item.eItemType == ItemType.Armor)
            {
                ArmorType armorType = ((Armor)item).eArmorType;

                unequipItem = Managers.Inventory.Find(i =>
                      i.m_bEquipped && i.eItemType == ItemType.Armor
                         && ((Armor)i).eArmorType == armorType);
            }

            if (unequipItem != null)
            {
                unequipItem.m_bEquipped = false;

                Managers.UIBattle.RefreshUI<UI_Inven>();

            }
        }
    }

    // 소모품 아이템
    public void UseItem(Item item)
    {
        Debug.Log("아이템 사용");

        // TODO 아이템 효과에 따라
        // 대분류 : consumable (포션, 퀘스트 아이템 등)
        // 분류 : potionEffect (포션 종류에 따라)
    }

    public void BuyItem(Item item, int count)
    {
        // 조건
        if(item.m_iPrice * count > m_iHaveMoeny)
        {
            Debug.Log("돈이 부족합니다.");
            Managers.UI.ClosePopupUI();
            return;
        }

        if(Managers.Inventory.GetEmptySlot() == null)
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

    public void RefreshAdditionalStat()
    {
        m_iWeaponDamage = 0;
        m_iArmorDefence = 0;

        foreach (Item item in Managers.Inventory.m_Items)
        {
            if (item.m_bEquipped == false)
                continue;

            switch (item.eItemType)
            {
                case ItemType.Weapon:
                    m_iWeaponDamage += ((Weapon)item).Damage;
                    break;
                case ItemType.Armor:
                    m_iArmorDefence += ((Armor)item).Defence;
                    break;
                case ItemType.Consumable:
                    break;
                default:
                    break;
            }
        }
    }

    public override void OnDead(GameObject Attacker)
    {
        base.OnDead(Attacker);

        m_strStat.m_iHp = m_strStat.m_iMaxHp;
        m_strStat.m_iMp = m_strStat.m_iMaxMp;
        eState = CreatureState.Idle;

        Managers.Battle.CheckPointLoad(gameObject);
    }

    public override void UpdateSound()
    {
        if (eState == CreatureState.Dead)
            SoundPlay("Player" + eState.ToString());
    }

    #region PlayerAction
    public IEnumerator Roll()
    {
        string animName = null;

        if (eState == CreatureState.Idle)
            animName = "Stand To Roll";
        else if (eState == CreatureState.Move)
            animName = "Run To Roll";

        PlayAnimation(animName);
        float time = GetAnimationTime(animName);
        Stop(time * 0.8f);
        eActionState = ActionState.Invincible;

        yield return new WaitForSeconds(time * 0.8f);
        PlayAnimation("Idle");
        yield break;
    }
    #endregion
}
