using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Alteruna;
using UnityEngine.Rendering;

public class GameManager : AttributesSync
{
    [SerializeField] private GameObject[] spawnLocationTeamA;
    [SerializeField] private GameObject[] spawnLocationTeamB;
    [SynchronizableField] private String player;
    private Health health;
    public TextMeshProUGUI healthText;
    private Multiplayer multiplayer;
    private void Awake(){
        multiplayer = GetComponent<Multiplayer>();
        multiplayer.SpawnAvatar(spawnLocationTeamA[0].transform);
    }
    private void Update (){
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
