using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Character : Base
{
    // 스탯 변화

    void SetStartStat()
    {
        MaxHp = Hp;
        MaxStamina = Stamina;
        OriginalAtk = Atk;

        m_strAttack.Init(gameObject);

        StartCoroutine(UpdateCoolTime());
        StartCoroutine(StaminaGraduallyFillingUp());
    }

    private void SetHp(int NewHp)
    {
        Hp = NewHp;
        if (Hp < 0)
        {
            Hp = 0;
            eState = Define.CreatureState.Dead;
        }

        Managers.UIBattle.StatUIRefersh();
        StartCoroutine(Managers.UIBattle.UIPlayerInfo.DownHP());
    }

    private void SetStamina(float NewSetStamina)
    {
        Stamina = Mathf.Clamp(NewSetStamina, 0, MaxStamina);
        Managers.UIBattle.StatUIRefersh();
    }

    IEnumerator UpdateCoolTime()
    {
        while (true)
        {
            m_fCoolTime -= Time.deltaTime;
            if (m_fCoolTime < 0)
                m_fCoolTime = 0;

            yield return null;
        }
    }

    public void SetStaminaGraduallyFillingUp(bool b)
    {
        if(b == true)
            StartCoroutine(StaminaGraduallyFillingUp());
        else
            StartCoroutine(StaminaGraduallyFillingUp(false));
    }

    IEnumerator StaminaGraduallyFillingUp(bool bStart = true)
    {
        if (bStart == false)
            yield return null;

        while (true)
        {
            Stamina += Time.deltaTime;
            SetStamina(Stamina);
            Managers.UIBattle.StatUIRefersh();

            yield return null;
        }
    }

    public virtual void SetMoveState(MoveState state)
    {
        if (eMoveState == state)
            return;

        switch (state)
        {
            case MoveState.None:
                eMoveState = MoveState.None;
                MoveSpeed = 0;
                break;
            case MoveState.Walk:
                eMoveState = MoveState.Walk;
                MoveSpeed = WalkSpeed;
                break;
            case MoveState.Run:
                eMoveState = MoveState.Run;
                MoveSpeed = RunSpeed;
                break;
            case MoveState.Crouch:
                eMoveState = MoveState.Crouch;
                MoveSpeed = CrouchSpeed;
                break;
            default:
                break;
        }

        UpdateAnimation();
    }

}
