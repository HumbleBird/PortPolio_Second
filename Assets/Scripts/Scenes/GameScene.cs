using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;
        Managers.Battle.SpawnCharater(Define.CharaterType.Player);
        Managers.Battle.SpawnCharater(Define.CharaterType.Boss);

        Managers.UIBattle.UIPlayerInfo = Managers.UI.ShowSceneUI<UI_PlayerInfo>();
        Managers.UIBattle.Init();

        Managers.Sound.Play("Bgm/bigbattle_2_FULL", Define.Sound.Bgm);
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
