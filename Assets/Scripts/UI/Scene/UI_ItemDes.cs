using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ItemDes : UI_Base
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public void RefreshUI()
    {
        if (_init == false)
            return;
    }
}
