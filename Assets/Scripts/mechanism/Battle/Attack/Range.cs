using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Range : Attack
{
    public override void AttackEnd()
    {
        throw new NotImplementedException();
    }

    public override void NormalAttack()
    {
        // 프로젝타일 오브젝트 소환

    }

    void Fire()
    {
		// 원거리
		// 조준
		if (Input.GetMouseButton(2))
		{
			bool AttackRedey = false;

			// 조준 애니메이션
			//AttackRedey() -> AttackRedey = true;

			// 시위 겨누기 1번 -> 완료되면 AttackRedey = true
			// 시위 겨누는 중 무한 반복

			// 공격 준비가 완료가 되었다면
			if (AttackRedey)
			{
				// 마우스 왼쪽 클릭 발사
				if (Input.GetMouseButtonUp(0))
				{
					//AttackEvent(FireArrow - m_iBasicAttackNum);
					AttackRedey = false;
				}
				// 조준 취소
				else if (Input.GetMouseButtonUp(2))
				{
					//CancelAttack();
					AttackRedey = false;
				}
			}
		}
	}
}
