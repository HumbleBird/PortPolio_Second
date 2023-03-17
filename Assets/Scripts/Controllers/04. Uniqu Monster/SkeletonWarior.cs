using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public partial class SkeletonWarior : Monster
{
    // 이새퀴는 공격 모션이 2개뿐임

    void Attack()
    {
        m_cAttack.NormalAttack();
    }
}
