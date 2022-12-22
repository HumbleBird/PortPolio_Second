using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : Character
{
    public Table_Player.Info playerInfo { get; set; }

    public override void SetInfo(int id)
    {
        playerInfo = Managers.Table.m_Player.Get(id);
        m_tStatInfo = Managers.Table.m_Stat.Get(playerInfo.m_iStat);
        ChangeClass(playerInfo.m_sClass);

        m_strCharacterAction.Init(gameObject);
    }

    protected override void UpdateDead()
    {
        Rigid.isKinematic = true;
        int deadId = UnityEngine.Random.Range(0, 2);
        Animator.Play($"Dead{deadId}");
        Managers.Object.Remove(ID);
        Destroy(gameObject, 5);
    }
}
