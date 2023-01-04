using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inven : UI_Popup
{
    enum GameObjects
    {
        ItemGridPannel
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));

        GameObject gridPannel = Get<GameObject>((int)GameObjects.ItemGridPannel);
        foreach (Transform child in gridPannel.transform)
            Managers.Resource.Destroy(child.gameObject);

        int InitItemPannelCount = 25;
        for (int i = 0; i < InitItemPannelCount; i++)
        {
            GameObject item = Managers.Resource.Instantiate("UI/Popup/UI_Inven_Item");
            item.transform.SetParent(gridPannel.transform);
        }

        gameObject.SetActive(false);

        return true;
    }

    public void RefreshUI()
    {
        if (_init == false)
            return;


    }
}
