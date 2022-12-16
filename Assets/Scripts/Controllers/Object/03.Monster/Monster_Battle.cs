using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public partial class Monster : AI
{
    int AttackNum = 101;
    protected override void AttackAction()
    {
        base.AttackAction();
        m_strAttack.AttackInfoCal(AttackNum);
    }
}
