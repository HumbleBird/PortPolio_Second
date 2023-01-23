using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        // 데이터

        // 프리팹 로드

        // UI
        Managers.UI.ShowSceneUI<UI_GameScene>();

        // 캐릭터
        Managers.Battle.SpawnCharater(Define.CharaterType.Player, true);


        //

        //Managers.Battle.SpawnCharater(Define.CharaterType.Monster);

        //Managers.Battle.SpawnCharater(Define.CharaterType.Boss);

        // After the player is summoned,
        //Managers.UIBattle.Init();
        //Managers.InputKey.Init();

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
