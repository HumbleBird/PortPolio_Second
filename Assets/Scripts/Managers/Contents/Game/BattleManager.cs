using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static Define;

// 사실상 게임 매니저
public class BattleManager
{
    GameObject root;
    GameObject monsterContainer;
    GameObject playerContainer;
    public Transform savePoint = null;
    public Dictionary<int, Transform> MonstersSpawnPos = new Dictionary<int, Transform>();

    public void Init()
    {
        CreateCreatureContainer();
        InitMonsterSpawn();
    }

    #region CheckPoint
    public void CheckPointLoad(GameObject go)
    {
        if(savePoint == null)
        {
            // 만약에 세이브를 처음 하지 않았다면
            GameObject startPoint = Managers.Resource.Instantiate("Objects/Other/StartPoint");
            go.transform.position = startPoint.transform.position;
        }
        else
        {
            go.transform.position = savePoint.transform.position;
        }
    }

    public void CheckPointSave(Transform pos)
    {
        savePoint.transform.position = pos.position;
    }


    #endregion

    #region Spawn
    public void InitMonsterSpawn()
    {
        // 맵 이름 얻기
        // 맵 이름에 맞는 몬스터 스폰 위치 전부 가져오기
        if (Managers.Scene.CurrentScene.SceneType == Scene.Game)
        {
            string path = "Assets/Resources/Prefabs/Objects/MonsterSpawnPoint/Dungeon";
            var files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                if (file.Contains(".meta"))
                    continue;

                path = file.Replace("\\", "/");
                path = path.Replace(".prefab", "");
                path = path.Replace("Assets/Resources/", "");
                GameObject go = Managers.Resource.Load<GameObject>(path);
                MonstersSpawnPos.Add(int.Parse(go.name), go.transform);
            }

            // 스폰할 위치랑 몬스터 매치하기
            foreach (var info in Managers.Table.m_MonsterSpawnPos.m_Dictionary)
            {
                if (info.Value.m_iDungeonType != (int)Managers.Scene.CurrentScene.SceneType)
                    return;

                foreach (var pos in MonstersSpawnPos)
                {
                    if(pos.Key == info.Value.m_iMonsterSpawnPosId)
                    {
                        GameObject go =  Spawn(ObjectType.Monster, info.Value.m_iMonsterId);
                        Base goBase = go.GetComponent<Base>();
                        goBase.Pos = pos.Value;
                    }
                }
            }
        }
    }

    public GameObject CreatePlayer(int id, bool myPlayer = false)
    {
        Table_Player.Info pinfo = Managers.Table.m_Player.Get(id);

        if (pinfo == null)
        {
            Debug.LogError("해당하는 Id의 플레이어가 없습니다.");
            return null;
        }

        // 소환
        GameObject go = Managers.Resource.Instantiate(pinfo.m_sPrefabPath);
        Managers.Object.Add(pinfo.m_nID, go);

        // 스탯
        Player pc = Util.GetOrAddComponent<Player>(go);
        pc.ID = id;
        pc.m_strStat.m_tStatInfo = Managers.Table.m_Stat.Get(pinfo.m_iStat);
        pc.eObjectType = ObjectType.Player;

        // 클래스
        pc.ChangeClass(pinfo.m_sClass);

        if (myPlayer == true)
        {
            Managers.Object.myPlayer = pc.GetComponent<MyPlayer>();
            MyPlayer myplayer = pc.GetComponent<MyPlayer>();
            Managers.Camera.Init();
            myplayer.m_FollwTarget = Managers.Resource.Instantiate("Objects/Camera/FollwTarget", go.transform);
        }

        return go;
    }

    public GameObject CreateMonster(int id)
    {
        Table_Monster.Info minfo = Managers.Table.m_Monster.Get(id);

        if (minfo == null)
        {
            Debug.LogError("해당하는 Id의 몬스터가 없습니다.");
            return null;
        }

        // 소환
        GameObject go = Managers.Resource.Instantiate(minfo.m_sPrefabPath);
        Managers.Object.Add(minfo.m_nID, go);

        // 스탯
        Monster monster = Util.GetOrAddComponent<Monster>(go);
        monster.ID = id;
        monster.m_strStat.m_tStatInfo = Managers.Table.m_Stat.Get(minfo.m_iStat);
        monster.eObjectType = ObjectType.Monster;

        // 클래스
        monster.ChangeClass(minfo.m_sClass);
        return go;
    }

    public GameObject CreateBossMonster(int id)
    {
        Table_Boss.Info binfo = Managers.Table.m_Boss.Get(id);

        if (binfo == null)
        {
            Debug.LogError("해당하는 Id의 보스가 없습니다.");
            return null;
        }

        // 소환
        GameObject go = Managers.Resource.Instantiate(binfo.m_sPrefabPath);
        Managers.Object.Add(binfo.m_nID, go);

        // 스탯
        Monster boss = go.GetComponent<Monster>();
        boss.ID = id;
        boss.m_strStat.m_tStatInfo = Managers.Table.m_Stat.Get(binfo.m_iStat);
        boss.eObjectType = ObjectType.Monster;

        // 클래스
        boss.ChangeClass(binfo.m_sClass);
        return go;
    }

    public GameObject Spawn(ObjectType type, int id, int delay = 0, bool myplayer = false)
    {
        GameObject go;

        switch (type)
        {
            case ObjectType.Player:
                go = CreatePlayer(id, myplayer);
                break;
            case ObjectType.Monster:
                go = CreateMonster(id);
                break;
            case ObjectType.Boss:
                go = CreateBossMonster(id);
                break;
            default:
                go = null;
                break;
        }

        CreatureInputContainer(go);

        return go;
    }
    
    public List<GameObject> Spawns(ObjectType type, int id, int count = 1, int delay = 0, bool myplayer = false)
    {
        List<GameObject> list = new List<GameObject>();
        GameObject go;

        for (int i = 0; i < count; i++)
        {
            switch (type)
            {
                case ObjectType.Player:
                    go = CreatePlayer(id, myplayer);
                    break;
                case ObjectType.Monster:
                    go = CreateMonster(id);
                    break;
                case ObjectType.Boss:
                    go = CreateBossMonster(id);
                    break;
                default:
                    go = null;
                    break;
            }

            CreatureInputContainer(go);
            list.Add(go);
        }

        return list;
    }

    public void SetPostionNearToPlayer(GameObject go)
    {
        Quaternion randomRotation = Quaternion.Euler(UnityEngine.Random.Range(0, 360), 0, 0);
        Player player =  Managers.Object.myPlayer;

        Vector3 result;
        if (GetRandomNavmeshLocation(player.transform.position, out result))
        {
            go.transform.position = result;
            go.transform.rotation = randomRotation;
        }

    }

    public bool GetRandomNavmeshLocation(Vector3 center, out Vector3 resultPostion, float radius = 10)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
        randomDirection += center;
        resultPostion = Vector3.zero;
        NavMeshHit anonymous;

        if (NavMesh.SamplePosition(randomDirection, out anonymous, radius, 1))
        {
            resultPostion = anonymous.position;
            return true;
        }

        return false;
    }

    #endregion

    #region Camera
    // 보스 한 정 카메라 무브
    void EndStage()
    {
        // 스테이지 blur
        // Cinemashin의 카메라와 shake가 충돌을 일으켜서 안 되는 듯.
        // 끝날 때만 이걸 활용하도록 한다.
        Managers.Camera.m_CameraEffect.ZoomEndStage(0f, -1.5f, 1.5f, 3f - 1.5f, 0.5f, Vector3.zero);
    }
    #endregion

    #region Battle

    public void RewardPlayer(Player player, Table_Reward.Info rewardData)
    {
        if (player == null || rewardData == null)
            return;

        int? slot = Managers.Inventory.GetEmptySlot();
        if (slot == null)
            return;

        Table_Item.Info info = Managers.Table.m_Item.Get(rewardData.m_iItemId);

        Item newItem =  Item.MakeItem(info);
        newItem.Count = rewardData.m_iCount;
        newItem.InventorySlot = slot.Value;

        // TODO
        //player.Inven.add(item);

        Managers.Inventory.Add(newItem);
        Managers.UIBattle.RefreshPopupUI<UI_Inven>();

    }

    #endregion

    #region Item
    // 장착 아이템
    public void EquipItem(Player player, Item equipItem)
    {
        if (player == null)
            return;

        player.EquipItem(equipItem);
        Managers.UIBattle.RefreshPopupUI<UI_Equipment>();

    }

    // 소모품 아이템
    public void UseItem(Player player, Item useItem)
    {
        if (player == null)
            return;

        player.UseItem(useItem);
    }
    #endregion

    #region Development Convenience

    public NPC m_npc;

    public IEnumerator NPCInteractionEventFunction()
    {
        while (true)
        {
            //if (Console.KeyAvailable == false)
                yield return null;
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                m_npc.Talk();
                yield break;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                m_npc.Interaction();
                yield break;
            }
            else if (Input.GetKey(KeyCode.Escape))
            {
                m_npc.EndInteraction();
                yield break;
            }

        }
    }

    public void CreateCreatureContainer()
    {
        root = Util.FindOrCreateGameObject("CreatureContainer");
        monsterContainer = Util.FindOrCreateGameObject("Monster");
        playerContainer = Util.FindOrCreateGameObject("Player");

        monsterContainer.transform.SetParent(root.transform);
        playerContainer.transform.SetParent(root.transform);
    }

    public void CreatureInputContainer(GameObject Creature)
    {
        ObjectType type = Creature.GetComponent<Base>().eObjectType;
        switch (type)
        {
            case ObjectType.None:
                break;
            case ObjectType.Player:
                Creature.transform.SetParent(playerContainer.transform);
                break;
            case ObjectType.Monster:
            case ObjectType.Boss:
                Creature.transform.SetParent(monsterContainer.transform);
                break;
            case ObjectType.Projectile:
                break;
            default:
                break;
        }
    }

    #endregion
}
