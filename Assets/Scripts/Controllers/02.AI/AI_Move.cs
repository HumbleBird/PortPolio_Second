using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Define;
using Random = UnityEngine.Random;

public partial class AI : Character
{
    #region 변수

    enum AIMoveMode
    {
        RandomMove, // 랜덤 이동
        WayPointMove // 지정 좌표 이동
    }

    AIMoveMode eAIMoveMode = AIMoveMode.RandomMove;

    NavMeshAgent navMeshAgent;                      
    LayerMask playerMask = (1 << (int)Layer.Player);
    LayerMask obstacleMask = (1 << (int)Layer.Obstacle);

    Vector3 playerLastPosition = Vector3.zero;      //  Last position of the player when was near the enemy
    Vector3 m_PlayerPosition = Vector3.zero;        //  Last position of the player when the player is seen by the enemy

    bool m_playerInRange = false;                //  플레이어가 시야 각 안에 있는가?
    bool m_IsPatrol = true;                      //  순찰 중인가?
    bool m_CaughtPlayer = false;                 //  if the enemy has caught the player

    float m_iAbsPlayerToAIRange = 2.5f;
    protected int m_MinAttackRange; // 최소 공격 거리
    int m_iNotChasePlayerRange;

    // WayPoint
    float startWaitTime = 4;                 //  Wait time of every action
    float m_WaitTime;                               //  딜레이 대기 시간

    float viewRadius = 5;                   //  Radius of the enemy view
    float viewAngle = 90;                    //  Angle of the enemy view

    public Transform[] waypoints;                   // 경로
    int m_CurrentWaypointIndex = 0;                 //  현재 경로

    // Random
    float m_RandomMoveRange = 10f;

    #endregion

    private void Chasing()
    {
        //  The enemy is chasing the player
        playerLastPosition = Vector3.zero;          //  Reset the player near position

        float dis = Vector3.Distance(transform.position, m_goTarget.transform.position);

        if (dis < m_MinAttackRange)
        {
            // 사정 거리 안에 들어왔으면 배틀 상태로 변경
            CaughtPlayer();
            return;
        }
        else
        {
            Move();
            navMeshAgent.SetDestination(m_PlayerPosition);
            m_CaughtPlayer = false;
        }

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)    //  플레이어의 위치까지 쫓아왔는데
        {
            // 플레이어의 마지막 위치에서 대기 시간이 끝났고, 플레이어를 못 잡았고, 인식 범위 밖으로 넘어갔다면
            // = 플레이어를 놓쳤을 경우
            if (m_WaitTime <= 0 && !m_CaughtPlayer && dis >= m_iNotChasePlayerRange)
            {
                //  초기화
                m_IsPatrol = true;
                m_goTarget = null;
                m_WaitTime = startWaitTime;
                Patroling();
            }
            else
            {
                //  Wait if the current position is not the player position
                if (dis >= m_iAbsPlayerToAIRange)
                    Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    void Patroling()
    {
        playerLastPosition = Vector3.zero;

        if (eAIMoveMode == AIMoveMode.WayPointMove)
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);    //  Set the enemy destination to the next waypoint

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            //  If the enemy arrives to the waypoint position then wait for a moment and go to the next
            if (m_WaitTime <= 0)
            {
                if(eAIMoveMode == AIMoveMode.RandomMove)
                {
                    if (RandomPoint(transform.position, m_RandomMoveRange, out Vector3 point)) //pass in our centre point and radius of area
                        navMeshAgent.SetDestination(point);
                }
                else if (eAIMoveMode == AIMoveMode.WayPointMove)
                {
                    m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
                    navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                }

                Move();
                m_WaitTime = startWaitTime;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
        else
            Move();
    }
    
    #region ExtraFunction

    void Stop()
    {
        navMeshAgent.isStopped = true;
        SetMoveState(MoveState.None);
    }
    
    void Move()
    {
        navMeshAgent.isStopped = false;

        if (!m_IsPatrol)
            SetMoveState(MoveState.Run);
        else
            SetMoveState(MoveState.Walk);
    }

    void CaughtPlayer()
    {
        Stop();
        LookingPlayer(m_PlayerPosition);
        m_CaughtPlayer = true;
        eState = CreatureState.Skill;
    }

    void LookingPlayer(Vector3 player)
    {
        // 플레이어를 바라봄
        transform.LookAt(player);
    }

    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);   //  Make an overlap sphere around the enemy to detect the playermask in the view radius

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);          //  Distance of the enmy and the player
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    m_playerInRange = true;             //  The player has been seeing by the enemy and then the nemy starts to chasing the player
                    m_IsPatrol = false;                 //  Change the eState to chasing the player
                }
                else //If the player is behind a obstacle the player position will not be registered
                    m_playerInRange = false;
            }

            if (Vector3.Distance(transform.position, player.position) > viewRadius) // 시야각 밖으로
                m_playerInRange = false;

            if (m_playerInRange)
            {
                m_PlayerPosition = player.transform.position;       //  Save the player's current position if the player is in range of vision
                m_goTarget = player.GetComponent<Player>();
            }
            else
                m_goTarget = null;
        }
    }

    public override void SetMoveState(MoveState state)
    {
        base.SetMoveState(state);

        navMeshAgent.speed = m_strStat.m_fMoveSpeed;
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
    #endregion
}