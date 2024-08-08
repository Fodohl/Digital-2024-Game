using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Alteruna;

public class GameManager : AttributesSync
{
    [SerializeField] private GameObject[] spawnLocationTeamA;
    [SerializeField] private GameObject[] spawnLocationTeamB;
    [] private String player;
    private Health health;
    public TextMeshProUGUI healthText;
    private void NewPlayer(){

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
