using UnityEngine;
using Alteruna;
using UnityEngine.SceneManagement;

public class testNetworkingJoin : CommunicationBridge
{
    public void Join(){
        Room room = Multiplayer.AvailableRooms[0];
        Multiplayer.JoinRoom(room);
        SceneManager.LoadScene("Game");
    }
    public void Host(){
        Multiplayer.CreateRoom();
        SceneManager.LoadScene("Game");
    }
}
