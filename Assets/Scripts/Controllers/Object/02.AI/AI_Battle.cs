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
            RandomAttack();
        }
        else
        {
            m_fCoolTime -= Time.deltaTime;
        }

        m_CaughtPlayer = false;

        // 타겟과의 거리 조사후 전 상태로 이동할지 말지 결정
    }

    protected virtual void RandomAttack()
    {
        // 랜덤 어택 및 상태 결정
    }
}