using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Player : Character
{
    #region Variable
    protected Vector3 m_MovementDirection;

    System.Diagnostics.Stopwatch m_FallingWatch =     new System.Diagnostics.Stopwatch();
    System.Diagnostics.Stopwatch m_AttackCheckWatch = new System.Diagnostics.Stopwatch();

    float m_fMiddleAttackTime = 0;
    float m_fEndAttackTime = 0;

    public override float m_TotalAttack { get { return m_Stat.m_iAtk + m_iWeaponDamage; } }
    public override float m_TotalDefence { get { return m_Stat.m_iDef + m_iArmorDefence; } }

    public int m_iHaveMoeny { get; private set; } = 10000;

    float m_fRotationSpeed = 10f;

    ItemSoket m_leftHandSlot;
    ItemSoket m_RightHandSlot;
    #endregion

    protected override void Init()
    {
        eObjectType = ObjectType.Player;

        base.Init();

        WeaponInit();

        Weapon weapon1 = Item.MakeItem(4) as Weapon;
        Weapon weapon2 = Item.MakeItem(5) as Weapon;
        LoadWeaponOnSlot(weapon1, true);
        LoadWeaponOnSlot(weapon2, false);
    }



    protected override void Update()
    {
        base.Update();
        HandleFalling();
        StaminaGraduallyFillingUp();
    }

    protected override void UpdateIdle()
    {
        base.UpdateIdle();

        if (m_MovementDirection != Vector3.zero)
        {
            if (m_bWaiting)
                return;

            SetMoveState(MoveState.Walk);
            eState = CreatureState.Move;
            return;
        }
    }

    // 걷기, 달리기 등
    protected override void UpdateMove()
    {
        if (m_bWaiting)
            return;

        // 앞에 장애물이 있다면 움직이지 못하게
        if (Physics.Raycast(transform.position, transform.forward, 0.4f))
        {
            m_MovementDirection = Vector3.zero;
        }

        // 이동 및 회전
        if (m_MovementDirection != Vector3.zero)
        {
            {
                transform.position += Time.deltaTime * m_Stat.m_fMoveSpeed * m_MovementDirection;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_MovementDirection), m_fRotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            eState = CreatureState.Idle;
        }
    }

    protected override void UpdateSkill()
    {
        base.UpdateSkill();

        // 공격 시간 체크

        // 첫 공격이라면
        if (m_AttackCheckWatch.Elapsed.Seconds == 0)
        {
            m_AttackCheckWatch.Start();
            m_fMiddleAttackTime = GetAnimationTime(m_cAttack.m_AttackInfo.m_sAnimName, 0.6f);
            m_fEndAttackTime = GetAnimationTime(m_cAttack.m_AttackInfo.m_sAnimName);
        }
        else
        {
            // 공격이 끝났다면
            if(m_AttackCheckWatch.Elapsed.Seconds >= m_fEndAttackTime)
            {
                eState = CreatureState.Idle;
                m_bCanAttack = true;
                m_bNextAttack = false;
                m_AttackCheckWatch.Reset();
                return;
            }
            // 다음 콤보 공격이 가능한 시간대
            else if(m_AttackCheckWatch.Elapsed.Seconds >= m_fMiddleAttackTime)
            {
                // 다음 콤보 공격
                if (m_bNextAttack == true && m_Stat.m_fStemina != 0 && m_cAttack.m_AttackInfo.m_iNextNum != 0)
                {
                    Managers.Battle.ExecuteEventDelegateAttackEnd();
                    Managers.Battle.ClearAllEvnetDelegate();
                    Managers.Battle.EventDelegateAttack += m_cAttack.NormalAttack;
                    m_AttackCheckWatch.Reset();
                    m_AttackCheckWatch.Start();
                    AttackEvent(m_cAttack.m_AttackInfo.m_iNextNum);
                    m_bNextAttack = false;
                }
            }
        }
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

            // 아이템 소켓에 아이템 장착

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
                    m_iArmorDefence += ((Armor)item).PhysicalResitance;
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
        if(Physics.Raycast(origin, -Vector3.up, out hit, minimunDistanceNeededToBeginFall, 1 << (int)Layer.Obstacle))
        {

            Vector3 tp = hit.point;
            targetPotion.y = tp.y;

            if(eMoveState == MoveState.Falling)
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

    protected override IEnumerator CoAttackCheck()
    {
        throw new NotImplementedException();
    }
}
