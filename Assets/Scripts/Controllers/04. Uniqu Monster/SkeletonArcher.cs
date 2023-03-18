using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public partial class SkeletonArcher : Monster
{
    // 이새퀴는 공격 모션이 2개뿐임

    void Attack()
    {
        m_cAttack.NormalAttack();
    }

    void Aim()
    {
        m_cAttack.SpeacialAction();
    }
}
