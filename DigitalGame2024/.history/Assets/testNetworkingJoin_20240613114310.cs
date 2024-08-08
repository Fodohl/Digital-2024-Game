using UnityEngine;
using Alteruna;
using UnityEngine.SceneManagement;

public class testNetworkingJoin : CommunicationBridge
{
    public void Join(){
        Room room = Multiplayer.AvailableRooms[0];
        Multiplayer.JoinRoom(room);
        Multiplayer.LoadScene("Game");
    }
    public void Host(){
        Multiplayer.CreateRoom();
        Multiplayer.LoadScene("Game");
    }
}
