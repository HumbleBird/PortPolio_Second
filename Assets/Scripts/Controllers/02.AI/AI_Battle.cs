using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public abstract partial class AI : Character
{
    protected override void UpdateSkill()
    {
        base.UpdateSkill();

        if (m_bCanAttack == true)
            StartCoroutine(ThinkAttackPattern());
    }

    IEnumerator ThinkAttackPattern()
    {
        while (true)
        {
            float dis = Vector3.Distance(transform.position, m_goTarget.transform.position);
            LookingPlayer(m_goTarget.transform.position);

            // 공격 사거리 안에 들었다면 공격
            if (dis <= navMeshAgent.stoppingDistance)
            {
                SetMoveState(MoveState.None);
                yield return StartCoroutine(AttackPattern());
            }
            else
            {
                // 플레이어 한테 이동
                Move();
                SetMoveState(MoveState.Run);

                // TODO 몬스터의 달리기 애니메이션
                navMeshAgent.SetDestination(m_goTarget.transform.position);

                // 완전히 시야 밖으로 나갔다면
                if (dis > m_iNotChasePlayerRange)
                {
                    m_bCanAttack = true;
                    AIMoveInit();
                    eState = CreatureState.Move;
                }
            }

            yield return null;
        }
    }

    protected abstract IEnumerator AttackPattern();

    protected override void AttackEvent(int id)
    {
        base.AttackEvent(id);

        // 공격 종료 체크
        StartCoroutine(CoAttackCheck());
    }

}