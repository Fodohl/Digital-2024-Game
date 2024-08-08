using System;
using TMPro;
using UnityEngine;
using Alteruna;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : AttributesSync
{
    [SerializeField] private GameObject[] spawnLocationTeamA;
    [SerializeField] private GameObject[] spawnLocationTeamB;
    [SerializeField] private GameObject references;
    [SerializeField] private bool inRoom = false;
    [SerializeField] private Health health;
    public TextMeshProUGUI healthText;

    private void Update()
    {
        if (inRoom)
        {
            if (Multiplayer.InRoom)
            {
                if (healthText == null){
                    healthText = FindObjectOfType<references>().GetComponent<references>().healthText;
                }
                if (health == null)
                {
                    var healthScripts = FindObjectsOfType<Health>();
                    foreach (var healthScript in healthScripts)
                    {
                        if (healthScript.gameObject.GetComponent<Alteruna.Avatar>().IsMe)
                        {
                            health = healthScript;
                        }
                    }
                }
                else
                {
                    healthText.text = "Health: " + health.currentHealth;
                }
                if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Game") && references == null){
                    references = FindObjectOfType<references>().gameObject;
                }
            }
        }
    }

    public void OnRoomJoined(Multiplayer multiplayerInstance, Room room, User user)
    {
        // inRoom = true;
        // Multiplayer.SpawnAvatar(new Vector3(0, 10, 0));
        // // if (ATeamUsers. > BTeamUsers) {

        // // }
    }
    public void SpawnAvatar(){
        inRoom = true;
        Multiplayer.SpawnAvatar(new Vector3(0, 10, 0));
        // if (ATeamUsers. > BTeamUsers) {

        // }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
