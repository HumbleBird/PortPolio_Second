using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : Character
{
    public Table_Player.Info playerInfo { get; set; }

    protected override void UpdateIdle()
    {
        base.UpdateIdle();
    }

    public override void SetInfo(int id)
    {
        playerInfo = Managers.Table.m_Player.Get(id);
        m_tStatInfo = Managers.Table.m_Stat.Get(playerInfo.m_iStat);
        ChangeClass(playerInfo.m_sClass);

        m_strCharacterAction.Init(gameObject);
    }
}
