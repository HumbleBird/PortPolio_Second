using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Attack : Strategy
{
    protected Table_Attack.Info info;

    public override void Init(GameObject go)
    {
        base.Init(go);

        // 공격 키 코드 넣기
        keyDictionary = new Dictionary<KeyCode, Action>()
        {
            //{ KeyCode.Mouse0, BasicAttack},
            //{ KeyCode.Mouse1, StrongAttack},
            //{ KeyCode.Z, Kick}
        };
    }

    public void One()
    {
        ;
    }

    public abstract void BasicAttack(int id = 1);
    public abstract void StrongAttack(int id = 4);
    public abstract void Skill();
    public virtual void Kick() { }
    public virtual void CanNextAttack(Action action, int id)
    {
        // action 함수 실행
    }
}
