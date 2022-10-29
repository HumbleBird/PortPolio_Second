using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Attack : Strategy
{
    public abstract void BasicAttack(int id = 1);
    public abstract void Skill();
    public virtual void Kick() { }
    public virtual void CanNextAttack(Action action, int id)
    {
        // action 함수 실행
    }
}
