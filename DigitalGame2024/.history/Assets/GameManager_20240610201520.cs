using System;
using TMPro;
using UnityEngine;
using Alteruna;
using UnityEngine.SceneManagement;

public class GameManager : AttributesSync
{
    [SerializeField] private GameObject[] spawnLocationTeamA;
    [SerializeField] private GameObject[] spawnLocationTeamB;
    [SynchronizableField] private String player;
    private Health health;
    public TextMeshProUGUI healthText;
    private void Awake(){
        DontDestroyOnLoad(gameObject);
    }
    private void Update (){
        if (Multiplayer.InRoom){
            if (health == null){
                var healthScripts =  FindObjectsOfType<Health>();
                foreach (var healthScript in healthScripts){
                    if (healthScript.gameObject.GetComponent<Alteruna.Avatar>().IsMe){
                        health = healthScript;
                    }
                }
            }else{
                healthText.text = "Health: " + health.currentHealth;
            }
        }
    }
    
    public void OnRoomJoined(Multiplayer multiplayerInstance, Endpoint endpointInstance, User user){
        Multiplayer.SpawnAvatar(spawnLocationTeamA[0].transform);
    }
    public void LoadScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }
}

