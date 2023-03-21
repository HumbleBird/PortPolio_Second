using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public class SkeletonArcher : Monster
{
    void Attack()
    {
        m_cAttack.NormalAttack();
    }

    void Aim()
    {
        m_cAttack.SpeacialAction();
    }

    protected override IEnumerator ThinkAttackPattern()
    {
        yield return null;
    }
}
