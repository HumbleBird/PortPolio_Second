using UnityEngine;
using System.Collections.Generic;

using EnhancedScrollerYachtDice.Room;

using Photon.Pun;
using Photon.Realtime;

public partial class PhotonMgr : MonoBehaviourPunCallbacks
{
    public void CreateLobbyRoom(string _strRoomName = null)//방 생성
    {
        if (null == _strRoomName)
            return;

        PhotonNetwork.CreateRoom(_strRoomName);

        Debug.Log("CreateLobbyRoom = " + _strRoomName);
    }

    public void RandomLobbyRoom()//랜덤으로 방 접속
    {
        PhotonNetwork.JoinRandomRoom();

        Debug.Log(" RandomLobbyRoom ");
    }

    public void JoinLobbyRoom(string _strRoomName = null)
    {
        if (null == _strRoomName)
            return;

        PhotonNetwork.JoinRoom(_strRoomName);

        Debug.Log("JoinLobbyRoom = " + _strRoomName);
    }

    public void LeveRoom(bool _bCom = true)
    {
        PhotonNetwork.LeaveRoom(_bCom);

        Debug.Log("LeveRoom = " + _bCom);
    }

    public void LeveLobby()
    {
        PhotonNetwork.LeaveLobby();

        Debug.Log(" LeveLobby = ");
    }

    public void SecretLobbyRoom(string _strRoomName, byte _bySecret, byte _byMaxPlayer)//비밀방
    {
        if (null == _strRoomName)
            return;

        bool bOpen = _bySecret > 0 ? false : true;

        RoomOptions roomoption = new RoomOptions() { IsVisible = bOpen, MaxPlayers = _byMaxPlayer };

        if (null == roomoption)
            return;

        SharedObject.g_SceneMgr.m_strRoomSelect = _strRoomName;
        SharedObject.g_SceneMgr.m_LobbyRoomSelect.m_eOwner = nsENUM.eTEAM.eTEAM_PLAYER1;
        SharedObject.g_SceneMgr.m_LobbyRoomSelect.m_byCount = _byMaxPlayer;

        PhotonNetwork.JoinOrCreateRoom(_strRoomName, roomoption, null);
    }

    public void SendRoomEntry()//방진입
    {
        PV.RPC("LobbyRoomEntry", RpcTarget.All, SharedObject.g_SceneMgr.m_LobbyRoomSelect.m_eOwner,
            SharedObject.g_SceneMgr.m_UserInfo.m_strPlayerName);
    }

    public void SendRoomReady()//ready완료
    {
        PV.RPC("LobbyRoomReady", RpcTarget.All, SharedObject.g_SceneMgr.m_LobbyRoomSelect.m_eOwner);
    }

    public void SendStartInGame()//게임시작
    {
        PV.RPC("StartInGame", RpcTarget.All);
    }

    public override void OnJoinedRoom()//방참가완료
    {
        base.OnJoinedRoom();

        UI_Unlimited ui = SharedObject.SerchUnlimited(nsENUM.eUI.eUI_UNLIMITED_ROOM);

        if (null == ui)
            return;

        UI_Unlimited_Room room = (UI_Unlimited_Room)ui;

        room.OnRoomReady();//ready방입장

        Debug.Log(" OnJoinedRoom ");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)//방 리스트
    {
        SharedObject.g_SceneMgr.m_bDicRoomUpdate = true;//갱신
        SharedObject.g_SceneMgr.m_DicRoom.Clear();//비효율적

        foreach (RoomInfo room in roomList)
        {
            SharedObject.g_SceneMgr.InsertRoomInfo(room.Name, room.MaxPlayers, (byte)room.PlayerCount);

            Debug.Log(" room = " + room.Name);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)//실패
    {
        base.OnJoinRoomFailed(returnCode, message);

        Debug.Log("OnJoinRoomFailed = " + message);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)//랜덤방 실패
    {
        base.OnJoinRandomFailed(returnCode, message);

        Debug.Log("OnJoinRandomFailed = " + message);
    }
}
