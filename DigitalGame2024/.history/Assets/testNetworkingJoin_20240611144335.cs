using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;

public class testNetworkingJoin : CommunicationBridge
{
    public void Join(){
        Room room = Multiplayer.AvailableRooms[0];
        Multiplayer.JoinRoom(room);
    }
    public void Host(){
        Multiplayer.CreateRoom();
    }
}
