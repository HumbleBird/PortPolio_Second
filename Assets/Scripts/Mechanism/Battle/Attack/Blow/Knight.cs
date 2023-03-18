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
    // TODO 버튼 한 번만 누르면 패링으로

    bool m_bShield = false; 

    // 쉴드 상태 돌입
    public override IEnumerator SpeacialAction()
    {
        if(!m_bShield)
        {
            string anim = "ShieldStart";
            m_cGo.PlayAnimation(anim);
            float time = m_cGo.GetAnimationTime(anim);

            if (m_cGo.eMoveState == MoveState.Run)
                m_cGo.SetMoveState(MoveState.Walk);

            yield return new WaitForSeconds(time);
            Shielding();

            yield break;
        }
    }

    // 쉴드 상태일 때 기능
    void Shielding()
    {
        m_cGo.eActionState = ActionState.Shield;
        m_bShield = true;
    }

    // 쉴드 종료
    public override IEnumerator SpeacialActionEnd()
    {
        if(m_bShield)
        {
            string anim = "ShieldEnd";
            m_cGo.PlayAnimation(anim);
            float time = m_cGo.GetAnimationTime(anim);
            m_cGo.eActionState = ActionState.None;
            m_bShield = false;

            yield return new WaitForSeconds(time);
            m_cGo.PlayAnimation("Idle");

            yield break;
        }

        yield return null;
    }
}
