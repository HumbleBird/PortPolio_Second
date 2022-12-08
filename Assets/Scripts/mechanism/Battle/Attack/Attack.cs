using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Attack : Strategy
{
    public Table_Attack.Info info;

    public abstract void BasicAttack(int id = 1);
    public abstract void StrongAttack(int id = 4);
    public abstract void Skill();
    public virtual void Kick() { }
}
