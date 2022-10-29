using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public partial class Monster : AI
{
    Table_Boss.Info bossInfo;

    public override void SetInfo(int id)
    {
        bossInfo = Managers.Table.m_Boss.Get(id);
        statInfo = Managers.Table.m_Stat.Get(bossInfo.m_iStat);
        aiInfo = Managers.Table.m_AI.Get(bossInfo.m_iAI);

        ChangeClass("Blow");
    }
}
