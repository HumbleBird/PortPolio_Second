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
        // 가벼운 공격
        // 무기 종류에 따라
        // AttackEvent인데.. AttackName(Animaion Name)과 Next Num 밖에 없음.

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
