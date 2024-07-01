using System.Collections;
using System.Collections.Generic;
using Alteruna;
using TMPro;
using UnityEngine;

public class references : CommunicationBridge
{
    public TextMeshProUGUI healthText;
    public GameObject[] uiElememts;
    public void Spawn() {
        FindObjectOfType<GameManager>().SpawnAvatar();
    }
}
