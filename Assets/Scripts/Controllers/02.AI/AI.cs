using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public partial class AI : Character
{
    protected override void Init()
    {
        base.Init();

        m_WaitTime = startWaitTime;                 //  Set the wait time variable that will change

        navMeshAgent = gameObject.GetOrAddComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = m_strStat.m_fMoveSpeed;             //  Set the navemesh speed with the normal speed of the enemy

        if (eCharacterClass == CharacterClass.Knight || eCharacterClass == CharacterClass.Warior)
        {
            navMeshAgent.stoppingDistance = 1.5f;
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

    protected bool DistanceMeasurementAttackRange()
    {
        if (m_goTarget == null)
            return false;

        // 몬스터 뒤에 플레이어가 있는 것을 알아야 되기 때문에 Target으로 
        float dis = Vector3.Distance(transform.position, m_goTarget.transform.position);
        if (dis <= navMeshAgent.stoppingDistance)
            return true;
        

        return false;
    }
}
