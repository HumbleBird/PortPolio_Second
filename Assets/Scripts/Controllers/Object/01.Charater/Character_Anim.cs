using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Character : Base
{
    void UpdateAnim()
    {
        switch (eState)
        {
            case CreatureState.Idle:
                Animator.Play()
                break;
            case CreatureState.Move:
                break;
            case CreatureState.Skill:
                break;
            case CreatureState.Dead:
                break;
            default:
                break;
        }
    }

}
