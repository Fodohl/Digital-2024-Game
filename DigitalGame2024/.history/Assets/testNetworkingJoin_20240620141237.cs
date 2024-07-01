using UnityEngine;
using Alteruna;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class testNetworkingJoin : CommunicationBridge
{
    List<Room> room = new List<Room>();
    [SerializeField] private InputField inputField;
    private float timer = 0;
    public void Join(){
        if (Multiplayer.IsConnected){
            Multiplayer.name = inputField.text;
            Multiplayer.JoinRoom(room[0]);
            Multiplayer.LoadScene("Game");
        }
    }
    public void Host(){
        if (Multiplayer.IsConnected){
            Multiplayer.name = inputField.text;
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
