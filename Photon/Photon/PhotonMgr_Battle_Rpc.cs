using UnityEngine;

using Photon.Pun;

public partial class PhotonMgr : MonoBehaviourPunCallbacks
{
    [PunRPC]
    void BattleTimeStart(bool _bPlayDice)
    {
        SharedObject.g_UIBattle.UIMONOBATTLE.InitUITime();//30초로 초기화
       
        if (SharedObject.g_BattleMgr.m_bInGameStart)
        {
            if (_bPlayDice)//시간 됐는데 안돌렸다면
            {
                SharedObject.g_BattleMgr.CheckDiceTurn();//횟수 감소

                if (!SharedObject.g_BattleMgr.CheckLeft())//횟수가 없으면 턴 변경
                    SharedObject.g_UIBattle.UIMONOBATTLE.SetTime(0);//게임오버와 팀변경
            }
        }
        else
            SharedObject.g_BattleMgr.m_bInGameStart = true;

        SharedObject.g_UIBattle.UIMONOBATTLE.GOCHANGE.SetActive(false);

        SharedObject.g_BattleMgr.OnSetTime();
    }

    [PunRPC]
    void BattleTeam(nsENUM.eTEAM _eTeam, bool _bInit)
    {
        if (SharedObject.g_BattleMgr.m_ePlayTeam != _eTeam)
            SharedObject.g_BattleMgr.ChangeTeam();//소유권 이전

        SharedObject.g_BattleMgr.m_ePlayTeam = _eTeam;

        SharedObject.g_UIBattle.UIMONOBATTLE.SetUIPlay(_eTeam);//플레이중 유저
        SharedObject.g_UIBattle.UIMONOBATTLE.OnUIRoll(_eTeam);

        if (!_bInit)//최초진입이 아닐때 교체
            SharedObject.g_UIBattle.UIMONOBATTLE.GOCHANGE.SetActive(true);
    }

    [PunRPC]
    void BattleDiceStart()
    {
        if(SharedObject.g_SceneMgr.m_LobbyRoomSelect.m_eOwner != SharedObject.g_BattleMgr.m_ePlayTeam)
            SharedObject.g_BattleMgr.StartDice();
    }

    [PunRPC]
    void BattleDiceEnd(nsENUM.eTEAM _eTeam, byte _byDice1, byte _byDice2, byte _byDice3, byte _byDice4, byte _byDice5)
    {
        if (_eTeam == SharedObject.g_SceneMgr.m_LobbyRoomSelect.m_eOwner)
            return;

        SharedObject.g_BattleMgr.SetDice(_byDice1, _byDice2, _byDice3, _byDice4, _byDice5);
    }

    [PunRPC]
    void BattleCatagoriesSave(nsENUM.eTEAM _eTeam, nsENUM.eGENEALOGY _e, byte _byScore)
    {
        if (SharedObject.g_SceneMgr.m_LobbyRoomSelect.m_eOwner != _eTeam)
            SharedObject.g_UIBattle.UIMONOBATTLE.SetUISelect(_eTeam, _e, _byScore);
    }

    [PunRPC]
    void BattleChoice(nsENUM.eTEAM _eTeam, int _nIndex)
    {
        Debug.Log("_eTeam = " + _eTeam + " _nIndex = " + _nIndex + 
            " owner = " + SharedObject.g_SceneMgr.m_LobbyRoomSelect.m_eOwner);

        if (SharedObject.g_SceneMgr.m_LobbyRoomSelect.m_eOwner != _eTeam)
            SharedObject.g_BattleMgr.SetLockDice(_nIndex);//주사위 상태 변경
    }

    [PunRPC]
    void BattleKeep(nsENUM.eTEAM _eTeam)
    {
        Debug.Log("_eTeam = " + _eTeam + " owner = " + SharedObject.g_SceneMgr.m_LobbyRoomSelect.m_eOwner);

        if (SharedObject.g_SceneMgr.m_LobbyRoomSelect.m_eOwner != _eTeam)
        {
            SharedObject.g_UIBattle.UIMONOBATTLE.GOCHOICEDICE.SetActive(false);
            //주사위가 안멈출때있다 그래서 강제 셋팅
            SharedObject.g_BattleMgr.SetKeepDice();//주사위 킵 상태

            SharedObject.g_UIBattle.UIMONOBATTLE.SetTime(0);//선택완료
        }
    }

    [PunRPC]
    void BattleGameOver(nsENUM.eTEAM _eTeam, bool _bOut)//나갈때도 호출됌
    {
        if(!_bOut)//그냥 나감
        {
            LeveRoom();//방나가기
            LeveLobby();

            SharedObject.g_SceneMgr.Scene = nsENUM.eSCENE.eSCENE_LOBBY;

            PhotonNetwork.LoadLevel((int)nsENUM.eSCENE.eSCENE_LOBBY);
        }  
        else//나는 안나갔으니 결과창보기
        {
            if (_eTeam == SharedObject.g_SceneMgr.m_LobbyRoomSelect.m_eOwner)
                return;

            UI_Unlimited ui = SharedObject.SerchUnlimited(nsENUM.eUI.eUI_UNLIMITED_RESULT);

            if (null == ui)
            {
                GameObject go = SharedObject.PrefabLoad(RSPath.OUTUI,
                           RSPath.AssetUI_Result, "Unlimited_Result",
                           SharedObject.g_UIBattle.TRDEPTH[(int)nsENUM.eUI_DEPTH.eUI_UNLIMITED]);

                ui = go.GetComponent<UI_Unlimited>();
            }

            if (null != ui)
                ui.gameObject.SetActive(true);
        }
    }

    [PunRPC]
    void BattleLogOut(nsENUM.eTEAM _eTeam)//상대방 나감
    {
        if (SharedObject.g_SceneMgr.m_LobbyRoomSelect.m_eOwner != _eTeam)
        {
            UI_Unlimited ui = SharedObject.SerchUnlimited(nsENUM.eUI.eUI_UNLIMITED_RESULT);

            if (null == ui)
            {
                GameObject go = SharedObject.PrefabLoad(RSPath.OUTUI,
                           RSPath.AssetUI_Result, "Unlimited_Result",
                           SharedObject.g_UIBattle.TRDEPTH[(int)nsENUM.eUI_DEPTH.eUI_UNLIMITED]);

                ui = go.GetComponent<UI_Unlimited>();
            }

            if (null != ui)
                ui.gameObject.SetActive(true);
        }
    }
}
