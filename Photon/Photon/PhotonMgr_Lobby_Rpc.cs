using UnityEngine;

using Photon.Pun;

public partial class PhotonMgr : MonoBehaviourPunCallbacks
{
    [PunRPC]
    void LobbyRoomEntry(nsENUM.eTEAM _e, string _strUserName)
    {
        if (_e != SharedObject.g_SceneMgr.m_LobbyRoomSelect.m_eOwner)//내가 아니면
        {
            SharedObject.g_SceneMgr.m_LobbyRoomPlayer.m_strUserName = _strUserName;

            UI_Unlimited ui = SharedObject.SerchUnlimited(nsENUM.eUI.eUI_UNLIMITED_ROOM_READY);

            if (null == ui)
                return;

            UI_Unlimited_RoomReady ready = (UI_Unlimited_RoomReady)ui;

            ready.OnUserRoomReady();

            if(nsENUM.eTEAM.eTEAM_PLAYER1 == SharedObject.g_SceneMgr.m_LobbyRoomSelect.m_eOwner)//
                SharedObject.g_PhotonMgr.SendRoomEntry();//방장정보 보내기
        }
    }

    [PunRPC]
    void LobbyRoomReady(nsENUM.eTEAM _e)//레디준비된 유저
    {
        if (_e == SharedObject.g_SceneMgr.m_LobbyRoomSelect.m_eOwner)//방장과 상대편여부에 따라
            SharedObject.g_SceneMgr.m_LobbyRoomSelect.m_bReady = true;
        else//상대편 접속
        {
            SharedObject.g_SceneMgr.m_LobbyRoomPlayer.m_bReady = true;

            UI_Unlimited ui = SharedObject.SerchUnlimited(nsENUM.eUI.eUI_UNLIMITED_ROOM_READY);

            if (null == ui)
                return;

            UI_Unlimited_RoomReady ready = (UI_Unlimited_RoomReady)ui;

            ready.BTNSTART.interactable = true; //시작버튼 활성화
        }

        if (SharedObject.g_SceneMgr.CheckLobbyRoomReady())
        {
            if (nsENUM.eTEAM.eTEAM_PLAYER1 == SharedObject.g_SceneMgr.m_LobbyRoomSelect.m_eOwner)//방장
                SendStartInGame();
        }
    }

    [PunRPC]
    void StartInGame()
    {
        SharedObject.g_SceneMgr.Scene = nsENUM.eSCENE.eSCENE_BATTLE;

        PhotonNetwork.LoadLevel((int)nsENUM.eSCENE.eSCENE_BATTLE);
    }
}
