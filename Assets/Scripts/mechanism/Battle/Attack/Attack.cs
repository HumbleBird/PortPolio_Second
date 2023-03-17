using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

public interface IBackStep
{
    IEnumerator BackStep();
}

public interface IMonsterSpawn
{
    IEnumerator MonsterSpawn();
}

public abstract class Attack
{
    public Character m_cGo;

    public void SetInfo(Character cha)
    {
        m_cGo = cha;
    }

    public abstract IEnumerator NormalAttack();
    public abstract IEnumerator SpeacialAction();
    public abstract void AttackEnd();


}
