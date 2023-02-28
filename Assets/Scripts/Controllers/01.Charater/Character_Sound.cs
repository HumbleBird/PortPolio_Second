using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Character : Base
{
    // character의 Idle 혹은 Dead 상태의 Sound Effect
    public void UpdateSound()
    {
        if (eState == CreatureState.Skill || eState == CreatureState.Move)
            return;

        SoundPlay(gameObject.name + " " + eState.ToString());
    }

    void SoundPlay(string soundName)
    {
        foreach (var sound in Managers.Table.m_Sound.m_Dictionary.Values)
        {
            if (soundName == sound.m_sName)
            {
                Managers.Sound.Play(sound.m_sPath, sound.m_iLoop, (Sound)sound.m_iSoundType);
                return;
            }
        }

        Debug.Log("Not Sound " + gameObject.name + soundName);
    }
}
