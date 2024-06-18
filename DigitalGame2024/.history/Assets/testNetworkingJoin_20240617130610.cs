using UnityEngine;
using Alteruna;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class testNetworkingJoin : CommunicationBridge
{
    List<Room> room = new List<Room>();
    private float timer = 0;
    public void Join(){
        print(room[0].Name);
        Multiplayer.JoinRoom(room[0]);
        Multiplayer.LoadScene("Game");
    }
    public void Host(){
        Multiplayer.CreateRoom();
        Multiplayer.LoadScene("Game");
    }
    private void Update() {
        timer += Time.deltaTime;
        print(timer);
        if (timer > 2){
            if(Multiplayer.IsConnected){
                room = Multiplayer.AvailableRooms;
            }
            timer = 0;
        }
    }
}
