using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public abstract  partial class Monster : AI
{
    protected List<RewardItemInfo> m_rewards = new List<RewardItemInfo>();

    public class RewardItemInfo
    {
        public Item m_Item;
        public int m_iCount;
        public float m_fProbability;
    }

    protected override void SetInfo()
    {
        eObjectType = ObjectType.Monster;
        gameObject.tag = "Monster";

        base.SetInfo();

        UIBar = Managers.UI.MakeWorldSpaceUI<UI_HpBar>(transform);

        if (eObjectType == ObjectType.Monster)
        {
            Table_Monster.Info info = Managers.Table.m_Monster.Get(ID);
            if (info == null)
            {
                Debug.LogError("해당하는 Id의 몬스터가 없습니다.");
                return;
            }

            ChangeClass(info.m_iClass);
            gameObject.layer = (int)Layer.Monster;
        }
        else if (eObjectType == ObjectType.Boss)
        {
            Table_Boss.Info info = Managers.Table.m_Boss.Get(ID);

            if (info == null)
            {
                Debug.LogError("해당하는 Id의 보스가 없습니다.");
                return;
            }

            ChangeClass(info.m_iClass);
            gameObject.layer = (int)Layer.Monster;
        }
    }

    public override void OnDead(GameObject attacker)
    {
        // 아이템 보상
        Base ba = attacker.GetComponent<Base>();

        if(ba.eObjectType == ObjectType.Player)
        {
            List<Item> rewardData = GetRandomRewards();
            if (rewardData != null)
            {
                foreach (Item item in rewardData)
                {
                    Player player = (Player)ba.GetOwner();
                    Managers.Battle.RewardPlayer(player, item);
                }
            }
        }

        eState = CreatureState.Dead;
    }

    List<Item> GetRandomRewards()
    {
        SetRewardItemInfo();

        int rand = Random.Range(0, 101);
        List<Item> items  = new List<Item>();

        foreach (var rewardData in m_rewards)
        {
            if(rand <= rewardData.m_fProbability)
            {
                items.Add(rewardData.m_Item);
            }
        }

        return items;
    }

    protected virtual void SetRewardItemInfo()
    {
        m_rewards.Clear();
    }

    public override void HitEvent(Character attacker, float dmg, bool isAnimation = true)
    {
        base.HitEvent(attacker, dmg, isAnimation);

        UIBar.RefreshUI();
    }
}
