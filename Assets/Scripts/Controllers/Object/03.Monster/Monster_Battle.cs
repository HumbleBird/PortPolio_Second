using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public partial class Monster : AI
{
    protected override void UpdateSkill()
    {
        base.UpdateSkill();

        if(m_fCoolTime <= 0)
        {
            _attack.BasicAttack(101);
        }
        else
        {
            m_fCoolTime -= Time.deltaTime;
        }
    }
}
