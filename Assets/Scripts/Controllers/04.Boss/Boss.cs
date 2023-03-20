using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public partial class Boss : Monster
{
    protected override void SetInfo()
    {
        Table_Boss.Info binfo = Managers.Table.m_Boss.Get(ID);

        if (binfo == null)
        {
            Debug.LogError("해당하는 Id의 보스가 없습니다.");
            return;
        }
        
       m_strStat.m_tStatInfo = Managers.Table.m_Stat.Get(ID);
       eObjectType = ObjectType.Boss;

        // 클래스
        ChangeClass(binfo.m_iClass);
    }
}
