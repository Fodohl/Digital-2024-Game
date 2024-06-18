using UnityEngine;
using Alteruna;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class testNetworkingJoin : CommunicationBridge
{
        List<Room> room = new List<Room>();
    public void Join(){
        print(room);
        Multiplayer.JoinRoom(room[0]);
        Multiplayer.LoadScene("Game");
    }
    public void Host(){
        Multiplayer.CreateRoom();
        Multiplayer.LoadScene("Game");
    }
    private void Update() {
        if(Multiplayer.IsConnected){
            room = Multiplayer.AvailableRooms;
        }
    }
}
