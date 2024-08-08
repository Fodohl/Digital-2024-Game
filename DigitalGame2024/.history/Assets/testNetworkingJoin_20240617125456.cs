using UnityEngine;
using Alteruna;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class testNetworkingJoin : CommunicationBridge
{
    public void Join(){
        List<Room> room = Multiplayer.AvailableRooms;
        print(room);
        Multiplayer.JoinRoom(room[0]);
        Multiplayer.LoadScene("Game");
    }
    public void Host(){
        Multiplayer.CreateRoom();
        Multiplayer.LoadScene("Game");
    }
}
