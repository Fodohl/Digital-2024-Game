using System.Collections;
using System.Collections.Generic;
using Alteruna;
using TMPro;
using UnityEngine;

public class references : CommunicationBridge
{
    public TextMeshProUGUI healthText;
    public void Spawn() {
        Multiplayer.SpawnAvatar();
    }
}
