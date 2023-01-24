using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BattleManager
{
    public UI_GameScene UIGameScene;


    public void LoadInvenItem()
    {
        UIGameScene = Managers.UI.SceneUI as UI_GameScene;

        UI_Inven UIInven = UIGameScene.UIInven;
        Managers.Inventory.Clear();

        // TODO
        // 메모리에 아이템 임시 생성
        int[] tempItemsId = { 1, 2, 101, 102, 103, 104, 201, 202 };

        for (int i = 0; i < tempItemsId.Length; i++)
        {
            Item item = Item.MakeItem(tempItemsId[i]);
            item.Count = 1;
            item.Slot = i;
            Managers.Inventory.Add(item);
        }

        // UI에 적용
        UIInven.RefreshUI();
    }

    public void StatUIRefersh()
    {
        UIGameScene = Managers.UI.SceneUI as UI_GameScene;

        UIGameScene.UIPlayerInfo.RefreshUI();
        UIGameScene.UIInven.RefreshUI();
    }
}
