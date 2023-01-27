using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

public class Arrow : Projectile
{
    public override Base GetOwner()
    {
        return Owner;
    }
}
