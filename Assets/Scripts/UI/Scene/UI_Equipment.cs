using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Equipment : UI_Base
{
    enum GameObjects
    {
        WeaponGrid,
        ShieldGrid,
        ArmorGrid,
        RingGrid,
        ItemGrid
    }

    int[] EquipmentInventoryCount = { 3, 3, 4, 4, 10 };

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));

        GameObject gridPannel = Get<GameObject>((int)GameObjects.WeaponGrid);
        foreach (Transform child in gridPannel.transform)
            Managers.Resource.Destroy(child.gameObject);

        for (int i = 0; i < 3; i++)
        {
            GameObject go = Managers.Resource.Instantiate("UI/Scene/UI_Equipment_Item", gridPannel.transform);
            UI_Equipment_Item item = go.GetOrAddComponent<UI_Equipment_Item>();
        }

        gameObject.SetActive(false);

        return true;
    }

    public void CreateEquipmentInventory(int count)
    {

    }
}
