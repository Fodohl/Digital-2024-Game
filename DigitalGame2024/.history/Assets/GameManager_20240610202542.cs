using System;
using TMPro;
using UnityEngine;
using Alteruna;
using UnityEngine.SceneManagement;

public class GameManager : AttributesSync
{
    [SerializeField] private GameObject[] spawnLocationTeamA;
    [SerializeField] private GameObject[] spawnLocationTeamB;
    [SynchronizableField] private User[] ATeamUsers;
    [SynchronizableField] private User[] BTeamUsers;

    private bool inRoom = false;
    [SynchronizableField] private String player;
    private Health health;
    public TextMeshProUGUI healthText;
    private void Awake(){
        DontDestroyOnLoad(gameObject);
    }
    private void Update (){
        if (inRoom){
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
    }
    
    public void OnRoomJoined(Multiplayer multiplayerInstance, Room room, User user){
        inRoom = true;
        Multiplayer.SpawnAvatar(spawnLocationTeamA[0].transform);
    }
    public void LoadScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }
}

