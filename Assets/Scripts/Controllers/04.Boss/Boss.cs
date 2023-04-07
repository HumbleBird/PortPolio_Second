using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public partial class Boss : Monster
{
    protected override void SetInfo()
    {
        base.SetInfo();

        if (eObjectType == ObjectType.Monster)
        {
            Table_Monster.Info info = Managers.Table.m_Monster.Get(ID);
            if (info == null)
            {
                Debug.LogError("해당하는 Id의 몬스터가 없습니다.");
                return;
            }

            ChangeClass(info.m_iClass);
            gameObject.layer = (int)Layer.Monster;
        }
        else if (eObjectType == ObjectType.Boss)
        {
            Table_Boss.Info info = Managers.Table.m_Boss.Get(ID);

            if (info == null)
            {
                Debug.LogError("해당하는 Id의 보스가 없습니다.");
                return;
            }

            ChangeClass(info.m_iClass);
            gameObject.layer = (int)Layer.Monster;
        }
    }

    protected override IEnumerator AttackPattern()
    {
        throw new System.NotImplementedException();
    }
}
