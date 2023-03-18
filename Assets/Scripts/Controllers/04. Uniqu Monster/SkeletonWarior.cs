using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public partial class SkeletonWarior : Monster
{
    // 이새퀴는 공격 패턴이 2개임
    // 달려가면서 공격하거나
    // 공격 타이밍이 1개 있는 단일 공격, 확률적으로 튀어 나오는 콤보 공격 2개

    // 단일 공격
    void Attack()
    {
        m_cAttack.NormalAttack();
    }

    // 달려가면서 공격 - 일정 거리에서 떨어진 상태에서
    void ForwardRunAttack()
    {

    }

    // 콤보 공격
    void ComboAttack()
    {
        m_cAttack.SpeacialAction();
    }
}
