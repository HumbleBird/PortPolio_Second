using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public partial class Monster : AI
{
    protected override void UpdateSkill()
    {
        base.UpdateSkill();

        if (m_bCanAttack == true)
            StartCoroutine(ThinkAttackPattern());
    }

    protected virtual IEnumerator ThinkAttackPattern()
    {
        yield return null;
    }
}
