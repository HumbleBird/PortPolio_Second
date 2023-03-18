using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Range : Attack
{
    public override IEnumerator AttackEnd()
    {
        throw new NotImplementedException();
    }

    public override IEnumerator NormalAttack()
    {
		// 장전되 있다면 Fire();
        throw new NotImplementedException();
    }

    public override IEnumerator SpeacialAction()
    {
        throw new NotImplementedException();
    }

    public override IEnumerator SpeacialActionEnd()
    {
        throw new NotImplementedException();
    }
}
