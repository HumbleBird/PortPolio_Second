using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public class SkeletonWarior : Monster
{
    protected override IEnumerator AttackPattern()
    {
        int rand = Random.Range(0, 2);
        if (rand == 0)
            yield return StartCoroutine(NormalAttack());
        else if (rand == 1)
            yield return StartCoroutine(ComboAttack());

        yield break;
    }

    // 단일 공격
    IEnumerator NormalAttack()
    {
        int rand = Random.Range(1011, 1013);
        float time = SkeletonWariorAttack(rand);
        yield return new WaitForSeconds(time);
        yield break;
    }


    // 콤보 공격
    IEnumerator ComboAttack()
    {
        int rand = Random.Range(1014, 1016);
        float time = SkeletonWariorAttack(rand);
        yield return new WaitForSeconds(time);
        yield break;
    }

    float SkeletonWariorAttack(int id)
    {
        Managers.Battle.EventDelegateAttack += m_cAttack.NormalAttack;
        AttackEvent(id);
        float time = GetAnimationTime(m_cAttack.m_AttackInfo.m_sAnimName);
        return time;
    }
}
