using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

// 방패 + 검
public class Knight : Blow
{
    // 쉴드
    public override IEnumerator SpeacialAction()
    {
        if (m_cGo.eMoveState == MoveState.Run)
            m_cGo.SetMoveState(MoveState.Walk);

        m_cGo.eActionState = ActionState.Shield;

        yield break;
    }
}
