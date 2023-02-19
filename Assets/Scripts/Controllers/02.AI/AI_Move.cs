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
    NavMeshAgent navMeshAgent;                      //  Nav mesh agent component
    float startWaitTime = 4;                 //  Wait time of every action
    float timeToRotate = 2;                  //  Wait time when the enemy detect near the player without seeing
    float m_WaitTime;                               //  딜레이 대기 시간
    float m_TimeToRotate;                           //  플레이어가 근처에 있을 때 딜레이 대기 시간

    float viewRadius = 5;                   //  Radius of the enemy view
    float viewAngle = 90;                    //  Angle of the enemy view

    public Transform[] waypoints;                   //  All the waypoints where the enemy patrols
    int m_CurrentWaypointIndex = 0;                     //  Current waypoint where the enemy is going to

    LayerMask playerMask;                    //  To detect the player with the raycast
    LayerMask obstacleMask;                  //  To detect the obstacules with the raycast
    int m_iPlayerLayer = 7;
    int m_iObstacleLayer = 11;

    Vector3 playerLastPosition = Vector3.zero;      //  Last position of the player when was near the enemy
    Vector3 m_PlayerPosition = Vector3.zero;        //  Last position of the player when the player is seen by the enemy

    bool m_playerInRange= false;                  //  If the player is in range of vision, eState of chasing
    bool m_PlayerNear  = false;               //  If the player is near, eState of hearing
    bool m_IsPatrol    = true;             //  If the enemy is patrol, eState of patroling

    protected bool m_CaughtPlayer= false;                 //  if the enemy has caught the player

    float range = 10f;
    #endregion

    private void Chasing()
    {
        //  The enemy is chasing the player
        m_PlayerNear = false;                       //  Set false that hte player is near beacause the enemy already sees the player
        playerLastPosition = Vector3.zero;          //  Reset the player near position

        if (m_goTarget == null)
            return;

        float dis = Vector3.Distance(transform.position, m_goTarget.transform.position);

        if (!m_CaughtPlayer)
        {
            Move();
            navMeshAgent.SetDestination(m_PlayerPosition);          //  set the destination of the enemy to the player location
        }

        // 적이 사정거리에 들면 공격
        // TODO
        if (dis < 2)
        {
            CaughtPlayer();
            return;
        }
        else
        {
            m_CaughtPlayer = false;
        }

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)    //  Control if the enemy arrive to the player location
        {
            // 플레이어를 놓쳤을 경우
            if (m_WaitTime <= 0 && !m_CaughtPlayer && dis >= 6f)
            {
                //  Check if the enemy is not near to the player, returns to patrol after the wait time delay
                m_IsPatrol = true;
                m_PlayerNear = false;
                m_goTarget = null;
                Move();
                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                if (eAIPatrolMode == AIPatrolMode.WayPoint)
                    navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                else if (eAIPatrolMode == AIPatrolMode.Random)
                    Patroling_RandomMove();
            }
            else
            {
                //  Wait if the current position is not the player position
                if (dis >= 2.5f)
                    Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    private void Patroling()
    {
        //m_PlayerNear 가 true인 경우는 hearing 함수를 만들었을 때
        if (m_PlayerNear)
        {
            //  Check if the enemy detect near the player, so the enemy will move to that position
            if (m_TimeToRotate <= 0)
            {
                Move();
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
            m_PlayerNear = false;           //  The player is no near when the enemy is platroling
            playerLastPosition = Vector3.zero;

            if (eAIPatrolMode == AIPatrolMode.WayPoint)
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);    //  Set the enemy destination to the next waypoint

            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                //  If the enemy arrives to the waypoint position then wait for a moment and go to the next
                if (m_WaitTime <= 0)
                {
                    if (eAIPatrolMode == AIPatrolMode.WayPoint)
                        NextPoint();
                    else if (eAIPatrolMode == AIPatrolMode.Random)
                        Patroling_RandomMove();

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
            {
                Move();
            }
        }
    }

    void Patroling_RandomMove()
    {
        Vector3 point;
        Vector3 centrePoint = transform.position;

        if (RandomPoint(centrePoint, range, out point)) //pass in our centre point and radius of area
        {
            navMeshAgent.SetDestination(point);
        }
    }

    #region ExtraFunction

    public void NextPoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    void Stop()
    {
        navMeshAgent.isStopped = true;
        SetMoveState(MoveState.None);
    }
    
    void Move()
    {
        navMeshAgent.isStopped = false;

        if (!m_IsPatrol)
        {
            SetMoveState(MoveState.Run);
        }
        else
        {
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
                Move();
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
                    m_goTarget = player.GetComponent<Player>();
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