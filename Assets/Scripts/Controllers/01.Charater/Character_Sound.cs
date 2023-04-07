using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public partial class Character : Base
{
    // 캐릭터가 가지는 효과음
    // Idle, Dead의 경우 해당 객체 이름 + 상태
    // Skill과 Move의 경우 Skill Attack Name과 Move는 걸을때마다 애니메이션 이벤트로 틀어주기

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
            //Debug.Log("Not Sound " + gameObject.name + " "+ soundName);
            return;
        }

        Managers.Sound.Play(info.m_sPath, info.m_iLoop, (Sound)info.m_iSoundType);
    }
}
