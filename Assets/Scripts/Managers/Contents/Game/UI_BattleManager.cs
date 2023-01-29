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

        // UI¿¡ Àû¿ë
        UIInven.RefreshUI();
    }

    public void StatUIRefersh()
    {
        UIGameScene = Managers.UI.SceneUI as UI_GameScene;

        UIGameScene.UIPlayerInfo.RefreshUI();
        UIGameScene.UIInven.RefreshUI();
    }

    public void UIInvenRefresh()
    {
        UIGameScene = Managers.UI.SceneUI as UI_GameScene;

        UI_Inven UIInven = UIGameScene.UIInven;
        UIInven.RefreshUI();
    }
}
