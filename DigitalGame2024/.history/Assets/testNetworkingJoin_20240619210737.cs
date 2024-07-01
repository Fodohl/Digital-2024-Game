using UnityEngine;
using Alteruna;
using System.Collections.Generic;

public class testNetworkingJoin : CommunicationBridge
{
    List<Room> room = new List<Room>();
    private float timer = 0;
    public void Join(){
        if (Multiplayer.IsConnected){
            Multiplayer.JoinRoom(room[0]);
            Multiplayer.LoadScene("Game");
        }
    }
    public void Host(){
        if (Multiplayer.IsConnected){
            Multiplayer.CreateRoom();
            Multiplayer.LoadScene("Game");
        }
    }
    private void Update() {
        timer += Time.deltaTime;
        if (timer > 1){
            if(Multiplayer.IsConnected){
                room = Multiplayer.AvailableRooms;
            }
            timer = 0;
        }
    }
}
