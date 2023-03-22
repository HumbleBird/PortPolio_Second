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
        //InitMonsterSpawn();
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
                        List<GameObject> go =  Spawn(info.Value.m_iMonsterId, ObjectType.Monster);
                        Base goBase = go[0].GetComponent<Base>();
                        goBase.Pos = pos.Value;
                    }
                }
            }
        }
    }

    public GameObject CreateCharacter(int id, ObjectType type)
    {
        // pool pattern
        if (Managers.Object.Find(id) != null)
            return Managers.Object.Find(id);

        string prefabPath = null;

        if(type == ObjectType.Player)
        {

            Table_Player.Info info = Managers.Table.m_Player.Get(id);

            if (info == null)
            {
                Debug.LogError("해당하는 Id의 플레이어가 없습니다.");
                return null;
            }

            prefabPath = info.m_sPrefabPath;
        }
        else if (type == ObjectType.Monster)
        {
            Table_Monster.Info info = Managers.Table.m_Monster.Get(id);

            if (info == null)
            {
                Debug.LogError("해당하는 Id의 몬스터가 없습니다.");
                return null;
            }

            prefabPath = info.m_sPrefabPath;

        }
        else if (type == ObjectType.Boss)
        {
            Table_Boss.Info info = Managers.Table.m_Boss.Get(id);

            if (info == null)
            {
                Debug.LogError("해당하는 Id의 보스가 없습니다.");
                return null;
            }

            prefabPath = info.m_sPrefabPath;
        }

        GameObject go = Managers.Resource.Instantiate(prefabPath);
        Character character = Util.GetOrAddComponent<Character>(go);
        character.ID = id;

        return character.gameObject;
    }

    public List<GameObject> Spawn(int id, ObjectType type, int count = 1)
    {
        List<GameObject> list = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject go = CreateCharacter(id, type);
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
        Managers.UIBattle.RefreshUI<UI_Inven>();
    }

    public void AddItemtoPlayer(Player player, Item newItem)
    {
        if (player == null || newItem == null)
            return;

        int? slot = Managers.Inventory.GetEmptySlot();
        if (slot == null)
            return;

        newItem.InventorySlot = slot.Value;

        // TODO
        //player.Inven.add(item);

        Managers.Inventory.Add(newItem);
        Managers.UIBattle.RefreshUI<UI_Inven>();
    }

    // 공격 했을 때
    public delegate void DelegateAttack();
    public event DelegateAttack EventDelegateAttack;

    // 피격 효과
    public delegate void DelegateHitEffect();
    public event DelegateHitEffect EventDelegateHitEffect;

    // 공격끝 났을 때 - Blow의 무기 콜라이더 끄기
    public delegate void DelegateAttackEnd();
    public event DelegateAttackEnd EventDelegateAttackEnd;


    public void ExecuteEventDelegateAttack()
    {
        if(EventDelegateAttack != null)
            EventDelegateAttack();
    }

    public void ExecuteEventDelegateHitEffect()
    {
        if(EventDelegateHitEffect != null)
           EventDelegateHitEffect();
    }

    public void ExecuteEventDelegateAttackEnd()
    {
        if(EventDelegateAttackEnd != null)
            EventDelegateAttackEnd();
    }

    public void ClearExecuteEventDelegateAttackEnd()
    {
        EventDelegateAttackEnd = null;
    }

    public void ClearAllEvnetDelegate()
    {
        EventDelegateAttack = null;
        EventDelegateHitEffect = null;
        EventDelegateAttackEnd = null;
    }

    #endregion

    #region Item
    // 장착 아이템
    public void EquipItem(Player player, Item equipItem)
    {
        if (player == null)
            return;

        player.EquipItem(equipItem);
        Managers.UIBattle.RefreshUI<UI_Equipment>();

    }

    // 소모품 아이템
    public void UseItem(Player player, Item useItem)
    {
        if (player == null)
            return;

        player.UseItem(useItem);
    }

    public void SelectItem(int id)
    {
        Debug.Log("인벤토리 창에서 아이템을 선택함" + id);
    }

    public void SellItem(int id)
    {
        // 플레이어한테 넣는게 낫겠음.
        // 조건 따지고

        // 구매 했으면
        // 구매할 건지 묻는 창 닫고
        Managers.UI.ClosePopupUI();

        // 돈 차감

        // 인벤토리에 넣기
    }

    public void Buytem(Player player, Item buyItem, int count)
    {
        if (buyItem == null || count <= 0)
            return;

        player.BuyItem(buyItem, count);
    }


    #endregion

    #region Development Convenience

    public delegate void ChainFunction();
    public event ChainFunction EVENTFunction;

    public void ExecutionEventFunction()
    {
        if (EVENTFunction != null)
            EVENTFunction();
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

    #region Player

    public void PlayerCanMove(bool can = true)
    {
        if(can)
        {
            Managers.Object.myPlayer.m_bWaiting = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Managers.Object.myPlayer.m_bWaiting = true;
            Managers.Object.myPlayer.eState = Define.CreatureState.Idle;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    #endregion
}
