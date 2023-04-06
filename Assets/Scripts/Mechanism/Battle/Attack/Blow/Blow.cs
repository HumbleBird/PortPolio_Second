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
    // 왼쪽 클릭 - 오른쪽 무기 - Leftattack
    // 오른쪽 클릭 - 왼쪽 장착 아이템 - 

    public override void NormalAttack()
    {
        TrigerDetector td = m_cGo.GetComponentInChildren<TrigerDetector>();
        if(td != null)
            td.Attack();
    }

    public override IEnumerator SpeacialAction()
    {
        yield return null;
    }

    public override IEnumerator SpeacialActionEnd()
    {
        yield return null;
    }
}
