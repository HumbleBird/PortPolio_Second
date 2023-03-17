using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using Random = UnityEngine.Random;

public partial class AI : Character
{
    protected override void UpdateSkill()
    {
        base.UpdateSkill();

        if(m_bCanAttack == true)
            DecideAttackNum();
    }

    void DecideAttackNum()
    {
        // TODO
        // int[] RandomAttackNum = { 1001, 1002, 1003 }; // 오크
        int[] RandomAttackNum = { 1011, 1012 }; //스켈레톤
        int RandomNum = Random.Range(0, RandomAttackNum.Length);
        AttackEvent(RandomAttackNum[RandomNum]);
    }

    protected override void HowNextAttack()
    {
        base.HowNextAttack();

        // AI는 확률적으로 공격할지 아니면 끝을 낼지를 함.
        int rand = Random.Range(0, 101);
        if (rand >= 50)
            ExcuteNextAttack(info.m_iNextNum);
    }
}