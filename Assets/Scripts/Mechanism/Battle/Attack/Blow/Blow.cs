using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

#region BlowPattern

public interface IShield
{
    IEnumerator Shield();
}

#endregion

public class Blow : Attack
{
    public override void NormalAttack()
    {
        TrigerDetector td = m_cGo.GetComponentInChildren<TrigerDetector>();
        td.Attack();
    }

    public override IEnumerator SpeacialAction()
    {
        yield return null;
    }

    public override IEnumerator SpeacialActionEnd()
    {
        yield return null;
    }
}
