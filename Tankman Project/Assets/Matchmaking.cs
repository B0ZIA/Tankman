using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Matchmaking : Photon.MonoBehaviour
{
    public static Matchmaking Instance;
    private static int roomIndex = 0;



    public static void JoinToAnyBattleOfType(BattleMode.Type battleType)
    {
        PhotonNetwork.JoinOrCreateRoom(battleType.ToString() + "_" + roomIndex, new RoomOptions(), new TypedLobby());
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void OnPhotonCreateRoomFailed()
    {
        roomIndex++;
        PhotonNetwork.JoinOrCreateRoom(GameManager.myMode.ToString() + "_" + roomIndex, new RoomOptions(), new TypedLobby());
    }

    private void OnPhotonJoinRoomFailed(object[] cause)
    {
        Debug.Log("OnPhotonJoinRoomFailed got called. This can happen if the room is not existing or full or closed.");
    }
}
