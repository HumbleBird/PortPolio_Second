using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class AI : Character
{
    protected override void UpdateSkill()
    {
        base.UpdateSkill();

        if (m_fCoolTime <= 0)
        {
            AttackAction();
        }

        m_CaughtPlayer = false;

        // 타겟과의 거리 조사후 전 상태로 이동할지 말지 결정
    }

    protected virtual void AttackAction()
    {
        // 어택 함수
    }
}