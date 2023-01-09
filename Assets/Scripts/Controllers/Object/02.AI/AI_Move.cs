﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Define;
using Random = UnityEngine.Random;

public partial class AI : Character
{
    public override void SetInfo(int id) { }

    #region 변수
    NavMeshAgent navMeshAgent;                      //  Nav mesh agent component
    public float startWaitTime = 4;                 //  Wait time of every action
    public float timeToRotate = 2;                  //  Wait time when the enemy detect near the player without seeing
    float m_WaitTime;                               //  딜레이 대기 시간
    float m_TimeToRotate;                           //  플레이어가 근처에 있을 때 딜레이 대기 시간

    public float Range;
    public float radius;

    public float viewRadius = 5;                   //  Radius of the enemy view
    public float viewAngle = 90;                    //  Angle of the enemy view

    public Transform[] waypoints;                   //  All the waypoints where the enemy patrols
    int m_CurrentWaypointIndex = 0;                     //  Current waypoint where the enemy is going to

    public LayerMask playerMask;                    //  To detect the player with the raycast
    public LayerMask obstacleMask;                  //  To detect the obstacules with the raycast

    Vector3 playerLastPosition = Vector3.zero;      //  Last position of the player when was near the enemy
    Vector3 m_PlayerPosition = Vector3.zero;        //  Last position of the player when the player is seen by the enemy

    bool m_playerInRange= false;                  //  If the player is in range of vision, eState of chasing
    bool m_PlayerNear  = false;               //  If the player is near, eState of hearing
    bool m_IsPatrol    = true;             //  If the enemy is patrol, eState of patroling

    protected bool m_CaughtPlayer= false;                 //  if the enemy has caught the player
    #endregion

    protected override void Start()
    {
        base.Start();

        m_WaitTime = startWaitTime;                 //  Set the wait time variable that will change
        m_TimeToRotate = timeToRotate;

        navMeshAgent = gameObject.GetOrAddComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = MoveSpeed;             //  Set the navemesh speed with the normal speed of the enemy
    }

    protected override void UpdateMove()
    {
        base.UpdateMove();

        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        if (!m_IsPatrol)
            Chasing();
        else
            Patroling();

        yield return null;
    }


    private void Chasing()
    {
        //  The enemy is chasing the player
        m_PlayerNear = false;                       //  Set false that hte player is near beacause the enemy already sees the player
        playerLastPosition = Vector3.zero;          //  Reset the player near position
        float dis = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);

        if (!m_CaughtPlayer)
        {
            Move(MoveState.Run);
            navMeshAgent.SetDestination(m_PlayerPosition);          //  set the destination of the enemy to the player location
        }

        // 적이 사정거리에 들면 공격
        if (dis < 2)
            CaughtPlayer();

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)    //  Control if the enemy arrive to the player location
        {
            // 플레이어를 놓쳤을 경우
            if (m_WaitTime <= 0 && !m_CaughtPlayer && dis >= 6f)
            {
                //  Check if the enemy is not near to the player, returns to patrol after the wait time delay
                m_IsPatrol = true;
                m_PlayerNear = false;
                Move(MoveState.Walk);
                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            }
            else
            {
                if (dis >= 2.5f)
                {
                    Stop();
                }
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    private void Patroling()
    {
        if (m_PlayerNear)
        {
            //  Check if the enemy detect near the player, so the enemy will move to that position
            if (m_TimeToRotate <= 0)
            {
                Move(MoveState.Walk);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                //  The enemy wait for a moment and then go to the last player position
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            //m_PlayerNear = false;           //  The player is no near when the enemy is platroling
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);    //  Set the enemy destination to the next waypoint
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                //  If the enemy arrives to the waypoint position then wait for a moment and go to the next
                if (m_WaitTime <= 0)
                {
                    NextPoint();
                    Move(MoveState.Walk);
                    m_WaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
            else
            {
                Move(MoveState.Walk);
            }
        }
    }

    #region ExtraFunction
    public void NextPoint()
    {
        Vector3 randomVector = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);

        if(eAIPatrolMode == AIPatrolMode.WayPoint)
        {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
        else if (eAIPatrolMode == AIPatrolMode.Random)
        {
            navMeshAgent.SetDestination(randomVector);
        }

    }

    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
        SetMoveState(MoveState.None);
    }
    
    void Move(MoveState state)
    {
        navMeshAgent.isStopped = false;

        if (!m_IsPatrol)
        {
            SetMoveState(MoveState.Run);
        }
        else
        {
            if(eActionState !=ActionState.None)
                m_strCharacterAction.ActionStateReset();
            SetMoveState(MoveState.Walk);
        }
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
        navMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (m_WaitTime <= 0)
            {
                m_PlayerNear = false;
                Move(MoveState.Walk);
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
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
                    m_goTarget = player.gameObject;
                }
                else
                {
                    /*
                     *  If the player is behind a obstacle the player position will not be registered
                     * */
                    m_playerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                /*
                 *  If the player is further than the view radius, then the enemy will no longer keep the player's current position.
                 *  Or the enemy is a safe zone, the enemy will no chase
                 * */
                m_playerInRange = false;                //  Change the sate of chasing
            }
            if (m_playerInRange)
            {
                /*
                 *  If the enemy no longer sees the player, then the enemy will go to the last position that has been registered
                 * */
                m_PlayerPosition = player.transform.position;       //  Save the player's current position if the player is in range of vision
            }
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero;

        return false;
    }

    public Vector3 GetRandomPoint(Transform point = null, float radius = 0)
    {
        Vector3 _point;

        if (RandomPoint(point == null ? transform.position : point.position, radius == 0 ? Range : radius, out _point))
        {
            Debug.DrawRay(_point, Vector3.up, Color.black, 1);

            return _point;
        }

        return point == null ? Vector3.zero : point.position;
    }

    public override void SetMoveState(MoveState state)
    {
        if (eMoveState == state)
            return;

        switch (state)
        {
            case MoveState.None:
                eMoveState = MoveState.None;
                navMeshAgent.speed = 0;
                break;
            case MoveState.Walk:
                eMoveState = MoveState.Walk;
                navMeshAgent.speed = WalkSpeed;
                break;
            case MoveState.Run:
                eMoveState = MoveState.Run;
                navMeshAgent.speed = RunSpeed;
                break;
            case MoveState.Crouch:
                eMoveState = MoveState.Crouch;
                navMeshAgent.speed = CrouchSpeed;
                break;
            default:
                break;
        }

        UpdateAnimation();
    }
    #endregion
}