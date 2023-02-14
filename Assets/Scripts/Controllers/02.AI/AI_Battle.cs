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
}