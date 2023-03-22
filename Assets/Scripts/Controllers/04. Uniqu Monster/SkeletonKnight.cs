using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public  class SkeletonKnight : Monster
{
    void Attack()
    {
        m_cAttack.NormalAttack();
    }

    public IEnumerator Shield()
    {
        StartCoroutine(m_cAttack.SpeacialAction());

        yield return null;
    }

    protected override IEnumerator AttackPattern()
    {
        yield return null;
    }
}
