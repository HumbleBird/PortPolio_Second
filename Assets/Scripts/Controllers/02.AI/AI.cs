using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public partial class AI : Character
{
    public enum AIMode
    {
        Stay, // 플레이어 감지 시 RandomMove로
        RandomMove, // 랜덤 이동
        WayPointMove // 지정 좌표 이동
    }

    public AIMode eAIMode = AIMode.Stay;

    protected override void Init()
    {
        base.Init();

        m_WaitTime = startWaitTime;                 //  Set the wait time variable that will change
        m_TimeToRotate = timeToRotate;

        navMeshAgent = gameObject.GetOrAddComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = m_strStat.m_fMoveSpeed;             //  Set the navemesh speed with the normal speed of the enemy

        playerMask = (1 << m_iPlayerLayer);
        obstacleMask = (1 << m_iObstacleLayer);
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

    float ChangeNextPatroltime = 3f;
    protected override void UpdateIdle()
    {
        base.UpdateIdle();

        if (m_playerInRange == true)
        {
            eState = CreatureState.Move;
            return;
        }

        ChangeNextPatroltime -= Time.deltaTime;

        if (ChangeNextPatroltime <= 0)
        {
            ChangeNextPatroltime = 3f;
            eState = CreatureState.Move;
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
}
