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

    float m_fRotationSpeed = 10f;

    //Battle
    System.Diagnostics.Stopwatch m_FallingWatch =     new System.Diagnostics.Stopwatch();
    System.Diagnostics.Stopwatch m_AttackCheckWatch = new System.Diagnostics.Stopwatch();

    float m_fMiddleAttackTime = 0;
    float m_fEndAttackTime = 0;

    float m_fWeaponDamage;
    float m_fArmorDefence;
    public override float m_TotalAttack { get { return m_fWeaponDamage; } }
    public override float m_TotalDefence { get { return m_fArmorDefence; } }

    protected Camera m_Camera;
    protected CameraController m_CameraController;

    protected float m_fVertical;
    protected float m_fHorizontal;
    protected float m_fMoveAmount;
    protected bool m_bSprint;
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

        Weapon weapon = Item.MakeItem(5) as Weapon;

        m_WeaponInRightHandSlots[0] = weapon;
        m_WeaponInLeftHandSlots[0] = weapon;

        Weapon weapon2 = Item.MakeItem(4) as Weapon;

        m_WeaponInRightHandSlots[1] = weapon2;
        m_WeaponInLeftHandSlots[1] = weapon2;

        m_LeftWeapon = m_UnarmedWeapon;
        m_RightWeapon = m_UnarmedWeapon;

        m_iCurrentRightWeaponIndex = -1;
        m_iCurrentLeftWeaponIndex = -1;

        m_Camera = Managers.Camera.m_Camera;
        m_CameraController = m_Camera.GetComponentInParent<CameraController>();
    }

    protected override void Update()
    {
        base.Update();


        HandleFalling();
        HandleQuickSlotsInput();
        StaminaGraduallyFillingUp();
        CheckInteractableObject();
        HandleLockOnInput();
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

        if(m_bLockOnFlag)
        {
            UpdateAnimatorValues(m_fVertical, m_fHorizontal, m_bSprint);
        }
        else
        {
            UpdateAnimatorValues(m_fMoveAmount, 0, m_bSprint);
        }

        // 이동 및 회전
        // 카메라를 향해 캐릭터 이동 방향 결정
        m_MovementDirection = Quaternion.AngleAxis(m_Camera.transform.rotation.eulerAngles.y, Vector3.up) * m_MovementDirection;

        transform.position += Time.deltaTime * m_Stat.m_fMoveSpeed * m_MovementDirection;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_MovementDirection), m_fRotationSpeed * Time.deltaTime);

        if (m_MovementDirection == Vector3.zero)
            eState = CreatureState.Idle;
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
                    //m_AttackCheckWatch.Start();

                    // 2콤보 이상부터 기존 데미지 1%씩 증가 => 2타 = 1타 데미지 * 0.01%, 3타 데미지 = 2타 데미지 * 0.01%
                    // 플레이어 한정
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
}
