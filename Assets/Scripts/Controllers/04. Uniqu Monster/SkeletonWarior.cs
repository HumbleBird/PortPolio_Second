using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public class SkeletonWarior : Monster
{
    protected override IEnumerator ThinkAttackPattern()
    {
        while (true)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0)
                yield return StartCoroutine(NormalAttack());
            else if (rand == 1)
                yield return StartCoroutine(ComboAttack());

            // 거리 측정
            if (DistanceMeasurementAttackRange())
                yield return null;
            else
            {
                eState = CreatureState.Move;
                AIMoveInit();
                yield break;
            }
        }
    }

    // 단일 공격
    IEnumerator NormalAttack()
    {
        AttackEvent(1011);
        float time = GetAnimationTime(m_cAttack.m_AttackInfo.m_sAnimName);
        Managers.Battle.EventDelegateAttack += () => { StartCoroutine(m_cAttack.NormalAttack()); };
        yield return new WaitForSeconds(time);
    }
    

    // 콤보 공격
    IEnumerator ComboAttack()
    {
        Managers.Battle.EventDelegateAttack += () => { StartCoroutine(m_cAttack.NormalAttack()); };
        AttackEvent(1011);
        float time = GetAnimationTime(m_cAttack.m_AttackInfo.m_sAnimName);
        // m_cAttack.NormalAttack(); // 함수 등록
        yield return new WaitForSeconds(time);

        Managers.Battle.EventDelegateAttack += () => { StartCoroutine(m_cAttack.NormalAttack()); };
        AttackEvent(1012);
        time = GetAnimationTime(m_cAttack.m_AttackInfo.m_sAnimName);
        // m_cAttack.NormalAttack(); // 함수 등록
        yield return new WaitForSeconds(time);
    }
}
