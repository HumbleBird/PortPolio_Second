using UnityEngine;

using Photon.Pun;

public partial class PhotonMgr : MonoBehaviourPunCallbacks
{
    public void SendBattleTimeStart() //주사위 옴기는 조건 있을때 활성화
    {
        bool bplaydice = false;

        bplaydice = SharedObject.g_BattleMgr.m_bPlayDice;

        PV.RPC("BattleTimeStart", RpcTarget.All, bplaydice);

        Debug.Log("SendBattleTimeStart = " + bplaydice);
    }

    public void SendBattleTeam(nsENUM.eTEAM _eTeam, bool _bInit)//공격팀에 관한 설정
    {
        PV.RPC("BattleTeam", RpcTarget.All, _eTeam, _bInit);

        Debug.Log("SendBattleTeam = " + _eTeam);
    }

    public void SendBattleDiceStart()
    {
        PV.RPC("BattleDiceStart", RpcTarget.All);
    }

    public void SendBattleDiceEnd(nsENUM.eTEAM _eTeam, byte _byDice1, byte _byDice2, byte _byDice3, byte _byDice4, byte _byDice5)//주사위 이동 끝
    {
        PV.RPC("BattleDiceEnd", RpcTarget.All, _eTeam, _byDice1, _byDice2, _byDice3, _byDice4, _byDice5);
    }

    public void SendBattleCatagoriesSave(nsENUM.eTEAM _eTeam, nsENUM.eGENEALOGY _e, byte _byScore)
    {
        PV.RPC("BattleCatagoriesSave", RpcTarget.All, _eTeam, _e, _byScore);//0회면
    }

    public void SendBattleChoice(nsENUM.eTEAM _eTeam, int _nIndex)
    {
        PV.RPC("BattleChoice", RpcTarget.All, _eTeam, _nIndex);
    }

    public void SendBattleKeep(nsENUM.eTEAM _eTeam)
    {
        PV.RPC("BattleKeep", RpcTarget.All, _eTeam);
    }

    public void SendBattleGameOver(nsENUM.eTEAM _eTeam, bool _bOut)//로비로
    {
        PV.RPC("BattleGameOver", RpcTarget.All, _eTeam, _bOut);//게임끝났을때 나갔는지 여부
    }

    public void SendBattleLogOut(nsENUM.eTEAM _eTeam)//종료
    {
        PV.RPC("BattleLogOut", RpcTarget.All, _eTeam);
    }
}
