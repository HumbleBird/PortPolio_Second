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

    protected override void SetRewardItemInfo()
    {
        base.SetRewardItemInfo();

        // 롱소드 1개 10%확률
        RewardItemInfo info1 = new RewardItemInfo();
        info1.m_Item = Item.MakeItem(1);
        info1.m_fProbability = 10f;
        info1.m_Item.Count = 1;
        m_rewards.Add(info1);

        // 체력 포션 20% (1개 80% 2개 20%)
        RewardItemInfo info2 = new RewardItemInfo();
        info2.m_Item = Item.MakeItem(201);
        info2.m_fProbability = 20f;
        int rand = Random.Range(0, 101);
        if(rand <= 80)
            info2.m_Item.Count = 1;
        else
            info2.m_Item.Count = 2;
        m_rewards.Add(info2);


    }
}
