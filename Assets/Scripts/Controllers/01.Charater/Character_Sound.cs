using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Character : Base
{
    // character의 Idle 혹은 Dead 상태의 Sound Effect
    public virtual void UpdateSound()
    {
        if (eState == CreatureState.Skill || eState == CreatureState.Move)
            return;

        SoundPlay(gameObject.name + " " + eState.ToString());
    }

    protected void SoundPlay(string soundName)
    {
        Table_Sound.Info info = Managers.Table.m_Sound.Get(soundName);

        if (info == null)
        {
            Debug.Log("Not Sound " + gameObject.name + " "+ soundName);
            return;
        }

        Managers.Sound.Play(info.m_sPath, info.m_iLoop, (Sound)info.m_iSoundType);
    }
}
