using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public abstract  partial class Monster : AI
{
    protected override void UpdateSkill()
    {
        base.UpdateSkill();

        if (m_bCanAttack == true)
            StartCoroutine(ThinkAttackPattern());
    }

    protected abstract IEnumerator ThinkAttackPattern();
}
