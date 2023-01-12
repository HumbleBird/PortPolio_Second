using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class BattleManager
{
    #region Spawn
    void CreatePlayer(int id)
    {
        Table_Player.Info pinfo = Managers.Table.m_Player.Get(id);

        if (pinfo == null)
        {
            Debug.LogError("해당하는 Id의 플레이어가 없습니다.");
            return;
        }

        // 소환
        GameObject go = Managers.Resource.Instantiate(pinfo.m_sPrefabPath);
        Managers.Object.Add(pinfo.m_nID, go);

        // 스탯
        Player pc = Util.GetOrAddComponent<Player>(go);
        pc.ID = id;
        pc.eCreatureType = CreatureType.Player;
        pc.m_strStat.m_tStatInfo = Managers.Table.m_Stat.Get(pinfo.m_iStat);
        pc.m_strStat.Init();

        // 클래스
        pc.ChangeClass(pinfo.m_sClass);
    }

    void CreateMonster(int id)
    {
        Table_Monster.Info minfo = Managers.Table.m_Monster.Get(id);

        if (minfo == null)
        {
            Debug.LogError("해당하는 Id의 몬스터가 없습니다.");
            return;
        }

        // 소환
        GameObject go = Managers.Resource.Instantiate(minfo.m_sPrefabPath);
        Managers.Object.Add(minfo.m_nID, go);

        // 스탯
        Monster monster = Util.GetOrAddComponent<Monster>(go);
        monster.ID = id;
        monster.eCreatureType = CreatureType.Player;
        monster.m_strStat.m_tStatInfo = Managers.Table.m_Stat.Get(minfo.m_iStat);
        monster.m_strStat.Init();

        // 클래스
        monster.ChangeClass(minfo.m_sClass);
    }

    void CreateBossMonster(int id)
    {
        Table_Boss.Info binfo = Managers.Table.m_Boss.Get(id);

        if (binfo == null)
        {
            Debug.LogError("해당하는 Id의 보스가 없습니다.");
            return;
        }

        // 소환
        GameObject go = Managers.Resource.Instantiate(binfo.m_sPrefabPath);
        Managers.Object.Add(binfo.m_nID, go);

        // 스탯
        Monster boss = go.GetComponent<Monster>();
        boss.ID = id;
        boss.eCreatureType = CreatureType.Player;
        boss.m_strStat.m_tStatInfo = Managers.Table.m_Stat.Get(binfo.m_iStat);
        boss.m_strStat.Init();

        // 클래스
        boss.ChangeClass(binfo.m_sClass);
    }

    public void SpawnCharater(CharaterType type)
    {
        switch (type)
        {
            case CharaterType.Player:
                CreatePlayer(1);
                break;
            case CharaterType.Monster:
                CreateMonster(201);
                break;
            case CharaterType.Boss:
                CreateBossMonster(101);
                break;
            default:
                break;
        }
    }
    #endregion

    #region Camera
    
    // 보스 한 정 카메라 무브
    void EndStage()
    {
        // 스테이지 blur
        // Cinemashin의 카메라와 shake가 충돌을 일으켜서 안 되는 듯.
        // 끝날 때만 이걸 활용하도록 한다.
        if (Input.GetKey(KeyCode.I))
        {
            Managers.Camera.ZoomEndStage(0f, -1.5f, 1.5f, 3f - 1.5f, 0.5f, Vector3.zero);
        }
    }
    #endregion

    #region ETC
    //마우스 커서
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    #endregion
}
