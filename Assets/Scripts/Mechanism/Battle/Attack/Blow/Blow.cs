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
    // 현재 장착한 무기 분류에 따라 애니메이션 분류
    // 왼쪽 클릭 - 오른쪽 무기 - Lightattack
    // 오른쪽 클릭 - 왼쪽 장착 아이템 - HeavyAttack 

    public override void NormalAction()
    {
        switch (m_eWeaponType)
        {
            case WeaponType.Daggers:
                m_cGo.AttackEvent(1);
                break;
            case WeaponType.StraightSwordsGreatswords:
                m_cGo.AttackEvent(4);
                break;
        }

        // 일반 무기의 경우(단검, 검) - 일반 공격
        TrigerDetector td = m_cGo.GetComponentInChildren<TrigerDetector>();
        if(td != null)
            td.Attack();
    }

    public override void SpecialAction()
    {
        // 일반 무기의 경우(단검, 검) - 일반 공격
        TrigerDetector td = m_cGo.GetComponentInChildren<TrigerDetector>();
        if (td != null)
            td.Attack();
    }

}
