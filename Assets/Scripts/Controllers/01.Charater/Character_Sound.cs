using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Character : Base
{
    public Table_Sound.Info m_tSoundInfo { get; set; } = new Table_Sound.Info();
    string clipName = null;

    public void GetAudioClicpName()
    {
        clipName = gameObject.name + " " + eState.ToString();

        foreach (var sound in Managers.Table.m_Sound.m_Dictionary)
        {   

        }

        if (eState == CreatureState.Move)
            clipName += " " + eMoveState.ToString();
    }

    public void UpdateSound()
    {
        if (eState == CreatureState.Skill || eState == CreatureState.Move)
            return;

        GetAudioClicpName();

        Managers.Sound.Play(m_tSoundInfo.m_sPath, m_tSoundInfo.m_iLoop, (Sound)m_tSoundInfo.m_iSoundType);
    }

    // 걸을 때
    public void Step()
    {

    }
}
