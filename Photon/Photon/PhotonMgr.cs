using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public partial class PhotonMgr : MonoBehaviourPunCallbacks
{
    public PhotonView PV;

    private void Awake()
    {
        if (SharedObject.g_PhotonMgr == null)
        {
            SharedObject.g_PhotonMgr = this;

            DontDestroyOnLoad(gameObject);
        }

        PhotonNetwork.GameVersion = "1.0.0";
        PhotonNetwork.SendRate = 20;//통신속도
        PhotonNetwork.SerializationRate = 10;//통신속도

        PhotonNetwork.ConnectUsingSettings();//접속

        Debug.Log(" Photon Connect ");
    }

    private void Start()
    { 
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        Debug.Log(" Photon OnDisconnected = " + cause);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log(" Photon OnConnectedToMaster ");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()//로비접속 성공
    {
        base.OnJoinedLobby();

        Debug.Log(" Photon OnJoinedLobby ");
    }

    public void OnLobby()//로비 조인
    {
        PhotonNetwork.IsMessageQueueRunning = true;

        Debug.Log(" Photon OnLobby ");
    }
}
