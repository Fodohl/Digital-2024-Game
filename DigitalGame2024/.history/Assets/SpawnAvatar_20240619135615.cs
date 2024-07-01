using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;

public class SpawnAvatar : CommunicationBridge
{
    public void Spawn() {
        print("pressed");
        Multiplayer.SpawnAvatar();
    }
}
