using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public partial class AI : Character
{
    protected UI_HpBar UIBar;

    protected override void Init()
    {
        base.Init();

        m_WaitTime = startWaitTime;                 //  Set the wait time variable that will change

        navMeshAgent = gameObject.GetOrAddComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = m_Stat.m_fMoveSpeed;             //  Set the navemesh speed with the normal speed of the enemy

        if (eCharacterClass == CharacterClass.Knight || eCharacterClass == CharacterClass.Warior)
        {
            navMeshAgent.stoppingDistance = 1.3f;
        }
        else if (eCharacterClass == CharacterClass.Archer || eCharacterClass == CharacterClass.Wizard)
        {
            navMeshAgent.stoppingDistance = 10f;

        }

        AIMoveInit();
    }

    protected override void UpdateController()
    {
        base.UpdateController();

        switch (eState)
        {
            case CreatureState.Idle:
                EnviromentView();
                break;
            case CreatureState.Move:
                EnviromentView();
                break;
        }
    }

    protected override void UpdateIdle()
    {
        base.UpdateIdle();

        if (m_playerInRange == true)
        {
            eState = CreatureState.Move;
            return;
        }
    }

    protected override void UpdateMove()
    {
        base.UpdateMove();

        if (!m_IsPatrol)
            Chasing();
        else
            Patroling();
    }

    protected override void SetInfo()
    {
        base.SetInfo();

        UIBar = Managers.UI.MakeWorldSpaceUI<UI_HpBar>(transform);

    }

    protected bool DistanceMeasurementAttackRange()
    {
        // Skill 상태에서 타겟이 뒤에 있어도 거리가 가깝다면 탐지 가능
        float dis = Vector3.Distance(transform.position, m_goTarget.transform.position);
        if (dis <= navMeshAgent.stoppingDistance)
            return true;

        return false;
    }

    protected override IEnumerator CoAttackCheck()
    {
        float time = GetAnimationTime(m_cAttack.m_AttackInfo.m_sAnimName);

        yield return new WaitForSeconds(time);

        AttackEnd();
        yield break;
    }
}
