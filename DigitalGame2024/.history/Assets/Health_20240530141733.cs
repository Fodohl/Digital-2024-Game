using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;

public class Health : AttributesSync {
    private const float startingHealth = 100f;
    public float currentHealth;
    
    private void Awake(){
        currentHealth = startingHealth;
    }
    private void Update(){
        if (currentHealth <= 0f){
            Die();
        }
    }
    public void TakeHealth(float amount){
        BroadcastRemoteMethod("TakeHealthBroadcast", amount);
    }
    public void GiveHealth(float amount){
        BroadcastRemoteMethod("GiveHealthBroadcast", amount);
    }
    [SynchronizableMethod]
    private void TakeHealthBroadcast(float amount){
        currentHealth -= amount;
    }
    [SynchronizableMethod]
    private void GiveHealthBroadcast(float amount){
        currentHealth += amount;
    }
    private void Die(){
        BroadcastRemoteMethod("DieBroadcast");
    }
    [SynchronizableMethod]
    private void DieBroadcast(){

    }
}
