using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public partial class AI : Character
{
    protected override void UpdateController()
    {
        base.UpdateController();

        switch (eState)
        {
            case CreatureState.Idle:
                EnviromentView();
                NextActionState();
                break;
            case CreatureState.Move:
                EnviromentView();
                break;
        }
    }

    float ChangeNextPatroltime = 3f;
    void NextActionState()
    {
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
}
