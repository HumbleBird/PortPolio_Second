using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

public class Blow : Attack
{
    protected override void BasicAttack()
    {
        WeaponColliderOn();
    }

    protected override void StrongAttack()
    {
        WeaponColliderOn();
    }

    protected void WeaponColliderOn()
    {
        // 콜라이더 활성화
        foreach (var DetectorCollider in m_cGo.m_GoAttackItem)
        {
            if (DetectorCollider.eAttackCollider == AttackCollider.Weapon)
            {
                DetectorCollider.AttackCanOn();
                return;
            }
        }
    }
}
