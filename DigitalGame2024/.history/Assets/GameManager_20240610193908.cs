using System;
using TMPro;
using UnityEngine;
using Alteruna;

public class GameManager : AttributesSync
{
    [SerializeField] private GameObject[] spawnLocationTeamA;
    [SerializeField] private GameObject[] spawnLocationTeamB;
    [SynchronizableField] private String player;
    private Health health;
    public TextMeshProUGUI healthText;
    private void Update (){
        if (Input.GetKeyDown(KeyCode.Space)){
            Multiplayer.SpawnAvatar(spawnLocationTeamA[0].transform);
        }
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