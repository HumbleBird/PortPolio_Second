using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Character : Base
{
    float m_fTime = 0.1f;
    int BaseLayer = 0;
    int UpperLayer = 1;
    protected float sprint;

    void UpdateAnim()
    {
        if()

        switch (eState)
        {
            case CreatureState.Idle:
                Animator.Play("Idle", BaseLayer, m_fTime);
                break;
            case CreatureState.Move:
                if(sprint == 0.5)
                    Animator.Play("Walk", BaseLayer, m_fTime);
                else
                    Animator.Play("Run", BaseLayer, m_fTime);
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
