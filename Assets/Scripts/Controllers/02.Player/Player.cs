using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Player : Character
{
    #region Variable
    protected Vector3 m_MovementDirection;

    System.Diagnostics.Stopwatch m_FallingWatch = new System.Diagnostics.Stopwatch();

    public int m_iWeaponDamage { get; private set; }
    public int m_iArmorDefence { get; private set; }

    public override int m_TotalAttack { get { return m_Stat.m_iAtk + m_iWeaponDamage; } }
    public override int m_TotalDefence { get { return m_Stat.m_iDef + m_iArmorDefence; } }

    public int m_iHaveMoeny { get; private set; }
    protected Coroutine cStaminaGraduallyFillingUp;

    float goundDetectionRayStartPoint = 0.5f;
    float minimunDistanceNeededToBeginFall = 1f;
    float groundDirectionRayDistance = 0.2f;

    [SerializeField]
    float fallingSpeed = 45f;
    #endregion

    protected override void Init()
    {
        eObjectType = ObjectType.Player;

        base.Init();

        cStaminaGraduallyFillingUp = StartCoroutine(StaminaGraduallyFillingUp());
        m_iHaveMoeny = 10000;
    }

    protected override void Update()
    {
        base.Update();

        float delta = Time.deltaTime;

        HandleFalling(delta);
    }

    public override void OnDead(GameObject Attacker)
    {
        base.OnDead(Attacker);

        m_Stat.m_iHp = m_Stat.m_iMaxHp;
        m_Stat.m_iMp = m_Stat.m_iMaxMp;
        eState = CreatureState.Idle;

        Managers.Battle.CheckPointLoad(gameObject);
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
    #endregion

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

    public override void UpdateSound()
    {
        if (eState == CreatureState.Dead)
            SoundPlay("Player" + eState.ToString());
    }

    #region PlayerAction
    public IEnumerator Roll()
    {
        string animName = "Run To Roll";

        PlayAnimation(animName);
        float time = GetAnimationTime(animName);
        Stop(time * 0.8f);
        eActionState = ActionState.Invincible;

        yield break;
    }

    public IEnumerator BackStep()
    {
        string animName = "BackStep";

        PlayAnimation(animName);
        float time = GetAnimationTime(animName);
        Stop(time * 0.8f);
        eActionState = ActionState.Invincible;

        yield break;
    }


    #endregion

    public void HandleFalling(float delta)
    {
        // 바닥으로 레이 캐스트를 쏴서 바닥에 부딪히면 착지, 아무것도 없으면 낙하 중.
        // 낙하 시간의 비례하여 데미지 받음, 2초? 정도 낙하라면 사망 처리 함.

        Vector3 origin = transform.position;
        origin.y += m_Collider.bounds.center.y;
        RaycastHit hit;

        // 앞에 장애물이 있다면 움직이지 못하게
        if (Physics.Raycast(transform.position, transform.forward, 0.4f))
        {
            m_MovementDirection = Vector3.zero;
        }

        if(eMoveState == MoveState.Falling)
        {
            m_Rigidbody.AddForce(-Vector3.up * fallingSpeed);
            m_Rigidbody.AddForce(m_MovementDirection * fallingSpeed / 10f);
        }

        Vector3 targetPotion = transform.position;

        Debug.DrawRay(origin, -Vector3.up * minimunDistanceNeededToBeginFall, Color.red, 0.1f, false);

        // 땅에 착지
        if(Physics.Raycast(origin, -Vector3.up, out hit, minimunDistanceNeededToBeginFall, ~(int)Layer.Obstacle))
        {
            Vector3 tp = hit.point;
            targetPotion.y = tp.y;

            if(eMoveState == MoveState.Falling)
            {
                //if (m_FallingWatch.Elapsed.TotalSeconds >= 2)
                //{
                //    Debug.Log("사망");
                //}
                //else
                if (m_FallingWatch.Elapsed.TotalSeconds >= 0.5)
                {
                    Debug.Log("you are flying the time : " + m_FallingWatch.Elapsed.TotalSeconds);
                    PlayAnimation("Falling To Landing");
                    eMoveState = MoveState.None;
                    float time = GetAnimationTime("Falling To Landing");
                    StartCoroutine(WaitToState(time, CreatureState.Idle));
                    m_FallingWatch.Stop();
                }
            }

            if (m_MovementDirection != Vector3.zero)
                transform.position = Vector3.Lerp(targetPotion, transform.position, Time.deltaTime);
            else
                transform.position = targetPotion;
        }
        // 낙하 중
        else
        {
            if (eMoveState != MoveState.Falling)
            {
                SetMoveState(MoveState.Falling);
                m_FallingWatch.Reset();
                m_FallingWatch.Start();
            }

            Vector3 vel = m_Rigidbody.velocity;
            vel.Normalize();
            m_Rigidbody.velocity = vel * (m_Stat.m_fWalkSpeed / 2);

        }
    }
}
