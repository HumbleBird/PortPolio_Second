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

        switch (State)
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

    float time = 3f;
    void NextActionState()
    {
        if (m_playerInRange == true)
        {
            State = CreatureState.Move;
            return;
        }

        time -= Time.deltaTime;

        if (time <= 0)
        {
            time = 3f;
            State = CreatureState.Move;
        }
    }
}
