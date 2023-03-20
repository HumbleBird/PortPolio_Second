using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

#region BlowPattern

public interface IShield
{
    IEnumerator Shield();
}

#endregion

public class Blow : Attack
{
    public override IEnumerator NormalAttack()
    {
        WeaponColliderOnOff(true);
        yield break;
    }

    public override IEnumerator AttackEnd()
    {
        WeaponColliderOnOff(false);
        yield break;
    }

    protected void WeaponColliderOnOff(bool OnOff)
    {
        foreach (var DetectorCollider in m_cGo.m_GoAttackItem)
        {
            if(OnOff)
            {
                 // 콜라이더 활성화
                if (DetectorCollider.eAttackCollider == AttackCollider.Weapon)
                {
                    DetectorCollider.AttackCan(OnOff);
                    return;
                }
            }
            else
            {
                DetectorCollider.AttackCan(OnOff);
                return;
            }
        }
    }

    public override IEnumerator SpeacialAction()
    {
        yield  break;

    }

    public override IEnumerator SpeacialActionEnd()
    {
        yield  break;
    }
}
