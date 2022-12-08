using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public partial class Monster : AI
{
    int AttackNum = 101;
    protected override void RandomAttack()
    {
        base.RandomAttack();
        //float dis = Vector3.Distance(transform.position, m_goTarget.transform.position);

        //if (m_strAttack.info.m_fAttackRange <= dis)

        m_strAttack.BasicAttack(AttackNum);
    }
}
