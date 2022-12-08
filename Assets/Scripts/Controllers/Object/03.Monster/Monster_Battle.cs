using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public partial class Monster : AI
{
    protected override void RandomAttack()
    {
        base.RandomAttack();

        m_strAttack.BasicAttack(101);
    }
}
