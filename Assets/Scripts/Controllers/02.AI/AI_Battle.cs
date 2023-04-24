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

        //StartCoroutine(ThinkAttackPattern());
    }

    IEnumerator ThinkAttackPattern()
    {
        while (true)
        {
            if (m_goTarget == null)
                yield break;

            float dis = Vector3.Distance(transform.position, m_goTarget.transform.position);
                
            // 공격 사거리 안에 들었다면 공격
            if (dis <= navMeshAgent.stoppingDistance)
            {
                LookingPlayer(m_goTarget.transform.position);
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
                    AIMoveInit();
                    eState = CreatureState.Move;
                }
            }

            yield return null;
        }
    }

    protected abstract IEnumerator AttackPattern();

    public override void AttackEvent(int id)
    {
        base.AttackEvent(id);

        // 공격 종료 체크
        StartCoroutine(CoAttackCheck());
    }

    protected IEnumerator CoAttackCheck()
    {
        float time = GetAnimationTime(m_cAttack.m_AttackInfo.m_sName);

        yield return new WaitForSeconds(time);

        AttackEnd();
        yield break;
    }

    public void ChangeClass(int ClassId)
    {
        eCharacterClass = (CharacterClass)ClassId;

        switch (eCharacterClass)
        {
            case CharacterClass.None:
                break;
            case CharacterClass.Warior:
                m_cAttack = new Warior();
                break;
            case CharacterClass.Knight:
                m_cAttack = new Knight();
                break;
            case CharacterClass.Archer:
                m_cAttack = new Archer();
                break;
            case CharacterClass.Wizard:
                m_cAttack = new Wizard();
                break;
            default:
                break;
        }

        m_cAttack.SetInfo(this);
    }
}