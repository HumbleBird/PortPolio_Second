using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public partial class SkeletonKnight : Monster, ISheild
{
    void Attack()
    {
        m_cAttack.NormalAttack();
    }

    public IEnumerator Shield()
    {
        StartCoroutine(m_cAttack.SpeacialAction());

        yield return null;
    }
}
