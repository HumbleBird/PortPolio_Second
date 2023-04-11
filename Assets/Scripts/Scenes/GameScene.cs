using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static Define;


public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        // 데이터
        Managers.Inventory.Clear();

        // 프리팹 로드 && 처음 지정 위치에 몬스터 스폰
        Managers.Battle.Init();
        
        // UI
        Managers.UI.ShowSceneUI<UI_GameScene>();
        Managers.UIBattle.Init();

#if DEBUG
        ClearLog();
#endif
    }

    public override void Clear()
    {
        
    }

    public void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
}
