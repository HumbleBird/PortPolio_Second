using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

// 방패 + 검
public class Archer : Range
{
    public override IEnumerator NormalAttack()
    {
        // 장전되 있다면 Fire();
        throw new NotImplementedException();
    }

    public override IEnumerator SpeacialAction()
    {
        //Aim();
        throw new NotImplementedException();
    }
}
