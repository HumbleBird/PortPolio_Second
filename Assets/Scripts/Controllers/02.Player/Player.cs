using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Player : Character
{
    #region Variable

    protected Vector3 m_MovementDirection;

    public int m_iHaveMoeny { get; private set; } = 10000;

    protected float m_fVertical;
    protected float m_fHorizontal;
    protected float m_fMoveAmount;
    private float m_fRotationSpeed = 10f;

    protected bool m_bSprint;
    public bool m_bLockOnFlag = false;
    protected bool m_bLockOnInput = true;

    protected Camera m_Camera;
    protected CameraController m_CameraController;

    protected bool m_bNextAttack = false;

    // Interact
    public UI_Interact UIInteract = null;
    public UI_Interact UIInteractPost = null;

    public ItemSoket m_leftHandSlot { get; private set; } // 왼손 장착 소켓
    public ItemSoket m_RightHandSlot { get; private set; } // 오른손 장착 소켓

    public Weapon m_LeftWeapon; // 현재 장착 아이템들 중 들고 있는 왼손 무기
    public Weapon m_RightWeapon; // 현재 장착 아이템들 중 들고 있는 오른손 무기
    public Weapon m_UnarmedWeapon; // 퀵 슬롯 중 빈 칸

    public int m_iCurrentRightWeaponIndex = -1;
    public int m_iCurrentLeftWeaponIndex = -1;

    float m_fWeaponDamage;
    float m_fArmorDefence;
    public override float m_TotalAttack { get { return m_fWeaponDamage; } }
    public override float m_TotalDefence { get { return m_fArmorDefence; } }

    // Attack
    System.Diagnostics.Stopwatch m_FallingWatch = new System.Diagnostics.Stopwatch();
    System.Diagnostics.Stopwatch m_AttackCheckWatch = new System.Diagnostics.Stopwatch();

    float m_fMiddleAttackTime = 0;
    float m_fEndAttackTime = 0;
    #endregion

    protected override void Init()
    {
        eObjectType = ObjectType.Player;
        gameObject.layer = (int)Layer.Player;

        base.Init();

        m_UnarmedWeapon = new Weapon();
        m_UnarmedWeapon.m_sPrefabPath = "Item/Weapons/Unarmed";
        m_UnarmedWeapon.m_bIsUnarmed = true;

        WeaponInit();

        {
            // TODO DELETE
            Weapon weapon = Item.MakeItem(5) as Weapon;

            Weapon weapon2 = Item.MakeItem(4) as Weapon;

            Managers.Battle.RewardPlayer(this, weapon);
            Managers.Battle.RewardPlayer(this, weapon2);
        }

        m_LeftWeapon = m_UnarmedWeapon;
        m_RightWeapon = m_UnarmedWeapon;

        m_iCurrentRightWeaponIndex = -1;
        m_iCurrentLeftWeaponIndex = -1;

        m_Camera = Managers.Camera.m_Camera;
        m_CameraController = m_Camera.GetComponentInParent<CameraController>();
    }

    protected override void Update()
    {
        HandleRotation();

        base.Update();

        HandleFalling();
        HandleQuickSlotsInput();
        HandleLockOnInput();

        StaminaGraduallyFillingUp();
        CheckInteractableObject();
    }

    #region UpdateState
    protected override void UpdateIdle()
    {
        base.UpdateIdle();

        if (m_bWaiting)
            return;

        if (m_MovementDirection != Vector3.zero)
        {
            SetMoveState(MoveState.Walk);
            eState = CreatureState.Move;
            return;
        }
        else
            UpdateAnimatorValues(0, 0, false);
    }

    protected override void UpdateMove()
    {
        base.UpdateMove();

        if (m_bWaiting)
            return;

        // 앞에 장애물이 있다면 움직이지 못하게
        if (Physics.Raycast(transform.position, transform.forward, 0.4f))
        {
            m_MovementDirection = Vector3.zero;
        }

        // Lock On에 따른 애니메이션 조정
        if(m_bLockOnFlag && m_bSprint == false)
        {
            UpdateAnimatorValues(m_fVertical, m_fHorizontal, m_bSprint);
        }
        else
        {
            UpdateAnimatorValues(m_fMoveAmount, 0, m_bSprint);
        }

        // 이동 및 회전
        m_MovementDirection = Quaternion.AngleAxis(m_Camera.transform.rotation.eulerAngles.y, Vector3.up) * m_MovementDirection;

        if (m_MovementDirection == Vector3.zero)
        {
            eState = CreatureState.Idle;
        }

        transform.position += Time.deltaTime * m_Stat.m_fMoveSpeed * m_MovementDirection;
    }

    protected override void UpdateSkill()
    {
        base.UpdateSkill();

        // 공격 시간 체크

        // 첫 공격이라면
        if (m_AttackCheckWatch.Elapsed.Seconds == 0)
        {
            m_AttackCheckWatch.Start();
            m_fMiddleAttackTime = GetAnimationTime(m_cAttack.m_AttackInfo.m_sName, 0.6f);
            m_fEndAttackTime = GetAnimationTime(m_cAttack.m_AttackInfo.m_sName);
        }
        else
        {
            // 공격이 끝났다면
            if(m_AttackCheckWatch.Elapsed.Seconds >= m_fEndAttackTime)
            {
                m_bCanAttack = true;
                m_bNextAttack = false;
                m_AttackCheckWatch.Reset();
                eState = CreatureState.Idle;
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
                    Managers.Battle.EventDelegateAttack += m_cAttack.NormalAction;

                    m_AttackCheckWatch.Reset();

                    // 2콤보 이상부터 기존 데미지 1%씩 증가 => 2타 = 1타 데미지 * 0.01%, 3타 데미지 = 2타 데미지 * 0.01%
                    m_fWeaponDamage = m_fWeaponDamage * 0.01f;
                    AttackEvent(m_cAttack.m_AttackInfo.m_iNextNum);
                    m_bNextAttack = false;
                }
            }
        }
    }

    public override void OnDead()
    {
        base.OnDead();

        // TODO 플레이어 세이브 포인트에서 다시 로드
        m_Stat.m_iHp = m_Stat.m_iMaxHp;
        m_Stat.m_iMp = m_Stat.m_iMaxMp;
        eState = CreatureState.Idle;

        Managers.Battle.CheckPointLoad(gameObject);
    }

    public override void SetMoveState(MoveState state)
    {
        base.SetMoveState(state);

        if (eMoveState == MoveState.Sprint)
            m_bSprint = true;
        else
            m_bSprint = false;
    }

    #endregion


}
