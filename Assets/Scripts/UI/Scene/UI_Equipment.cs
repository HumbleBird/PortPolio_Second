using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_Equipment : UI_Base
{
    enum GameObjects
    {
        WeaponGrid,
        RightProjectileGrid,
        ShieldGrid,
        LeftProjectileGrid,
        ArmorGrid,
        SpeicialGrid,
        RingGrid,
        ItemGrid
    }

    int[] EquipmentInventoryCount = { 3, 2, 3, 2, 4, 1, 4, 10 };

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));

        for (int i = 0; i < EquipmentInventoryCount.Length; i++)
        {
            CreateEquipmentInventory(EquipmentInventoryCount[i], i);
        }

        gameObject.SetActive(false);

        return true;
    }

    public void CreateEquipmentInventory(int count, int enumIndex)
    {
        GameObject gridPannel = GetObject(enumIndex);
        foreach (Transform child in gridPannel.transform)
            Managers.Resource.Destroy(child.gameObject);

        for (int i = 0; i < count; i++)
        {
            GameObject go = Managers.Resource.Instantiate("UI/Scene/UI_Equipment_Item", gridPannel.transform);
            UI_Equipment_Item item = go.GetOrAddComponent<UI_Equipment_Item>();

            item.eEquimentItemCategory = (EquimentItemCategory)enumIndex;
        }
    }

}
