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
        Managers.Battle.EventDelegateAttack += () => { StartCoroutine(m_cAttack.NormalAttack()); };
        AttackEvent(id);
        float time = GetAnimationTime(m_cAttack.m_AttackInfo.m_sAnimName);
        Debug.Log($"현재 애니메이션 : {m_cAttack.m_AttackInfo.m_sAnimName}, 애니메이션 길이 {time}");
        Debug.Log(m_bCanAttack);
        return time;
    }
}
