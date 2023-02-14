using System.Collections;
using System.Collections.Generic;
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

        // 프리팹 로드
        Managers.Battle.Init();

        // 캐릭터
        Managers.Battle.Spawn(ObjectType.Player, 1, 1, 0, true);

        List<GameObject> list = Managers.Battle.Spawn(ObjectType.Monster, 201, 5, 0, true);

        foreach (var go in list)
            Managers.Battle.SetPostionNearToPlayer(go);

        // UI
        Managers.UI.ShowSceneUI<UI_GameScene>();
        Managers.UIBattle.Init();


        //bgm
        //Managers.Sound.Play("Bgm/bigbattle_2_FULL", Define.Sound.Bgm);

        //Managers.UI.ShowSceneUI<UI_Inven>();
        //gameObject.GetOrAddComponent<CursorController>();

        //GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, "UnityChan");
        //Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);

        ////Managers.Game.Spawn(Define.WorldObject.Monster, "Knight");
        //GameObject go = new GameObject { name = "SpawningPool" };
        //SpawningPool pool = go.GetOrAddComponent<SpawningPool>();
        //pool.SetKeepMonsterCount(2);
    }

    public override void Clear()
    {
        
    }
}
