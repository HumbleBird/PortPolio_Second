using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public partial class Monster : AI
{
    List<Table_Reward.Info> m_rewards = new List<Table_Reward.Info>();

    protected override void Init()
    {
        base.Init();

        eObjectType = ObjectType.Monster;
        m_strStat.m_iHp = 30;
    }

    public override void OnDead(GameObject attacker)
    {
        // 아이템 보상
        Base ba = attacker.GetComponent<Base>();

        if(ba.eObjectType == ObjectType.Player)
        {
            Table_Reward.Info rewardData = GetRandomReward();
            if(rewardData != null)
            {
                Player player = (Player)ba.GetOwner();
                Managers.Battle.RewardPlayer(player, rewardData);
            }
        }

        eState = CreatureState.Dead;
    }

    Table_Reward.Info GetRandomReward()
    {
        GetMonsterRewardItemIdsList();

        int rand = Random.Range(0, 101);

        float sum = 0f;
        foreach (var rewardData in m_rewards)
        {
            sum += rewardData.m_fProbability;
            if(rand <= sum)
            {
                return rewardData;
            }
        }

        return null;
    }

    void GetMonsterRewardItemIdsList()
    {
        m_rewards.Clear();
        int[] tempIds = null;

        // Monster Reward Ids
        int[] SkeletonRewardIds = { 1, 2, 3 };


        if (ID == 201)
        {
            tempIds = SkeletonRewardIds;
        }

        for (int i = 0; i < tempIds.Length; i++)
        {
            m_rewards.Add(Managers.Table.m_Reward.Get(tempIds[i]));
        }
    }


}
