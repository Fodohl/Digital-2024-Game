using UnityEngine;
using Alteruna;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Cinemachine;

public class MainMenuManager : AttributesSync
{
    List<Room> room = new List<Room>();
    public Multiplayer multiplayer;
    private float timer = 0;
    public GameObject connectingScreen;
    [SerializeField] private CinemachineVirtualCamera[] views;
    [SerializeField] private TMP_InputField nameField;
    private void Awake(){
        connectingScreen.SetActive(true);
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
    public void ExitGame(){
        Application.Quit();
    }
    public void OpenSettings(){
        views[0].Priority = 0;
        views[1].Priority = 1;
    }
    public void CloseSettings(){
        views[0].Priority = 1;
        views[1].Priority = 0;
        if (nameField.text != null){
            Multiplayer.SetUsername(nameField.text);
        }
    }
}
