using UnityEngine;
using Alteruna;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class MainMenuManager : AttributesSync
{
    List<Room> room = new List<Room>();
    [SerializeField] private TMP_InputField inputField;
    public Multiplayer multiplayer;
    private float timer = 0;
    public GameObject connectingScreen;
    private void Awake(){
        if (multiplayer.IsConnected){
            connectingScreen.SetActive(false);
        } else {
            multiplayer.OnConnected.AddListener(OnConnected);
        }
    } 
    public void Join(){
        if (Multiplayer.IsConnected){
            Multiplayer.JoinFirstAvailable();
            Multiplayer.LoadScene("Game");
        }
    }
    private void OnConnected(Multiplayer multiplayer, Endpoint end){
        connectingScreen.SetActive(false);
    }
    public void Host(){
        if (Multiplayer.IsConnected){
            Multiplayer.CreateRoom(Multiplayer.GetUser().Name + "'s room");
            Multiplayer.LoadScene("Game");
        }
    }
    private void Update() {
        timer += Time.deltaTime;
        if (timer > 1){
            if(Multiplayer.IsConnected){
                connectingScreen.SetActive(false);
                room = Multiplayer.AvailableRooms;
            }
            timer = 0;
        }
    }
}
