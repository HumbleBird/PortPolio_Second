using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : Charater
{
    public Table_Player.Info playerInfo { get; set; }

    public GameObject _attackItem;
    protected ActionMove m_stPlayerMove = new ActionMove();

    public override void SetInfo(int id)
    {
        playerInfo = Managers.Table.m_Player.Get(id);
        statInfo = Managers.Table.m_Stat.Get(playerInfo.m_iStat);
        ChangeClass(playerInfo.m_sClass);

        m_stPlayerMove.Init(gameObject);
    }


}
