using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;

public class Health : AttributesSync {
    private const float startingHealth = 100f;
    [SynchronizableField] public float currentHealth;
    
    private void Awake(){
        currentHealth = startingHealth;
    }
    public void TakeHealth(float amount, Alteruna.User enemyUser){
        currentHealth -= amount;
        if (currentHealth <= 0f){
            Die();
        }
        BroadcastRemoteMethod("UpdateUIBroadcast");
    }
    public void GiveHealth(float amount){
        currentHealth += amount;
        BroadcastRemoteMethod("UpdateUIBroadcast");
    }
    [SynchronizableMethod]
    private void UpdateUIBroadcast(){
        GameUIManager.Instance.UpdateUI();
    }
    private void Die(){
        GameManager.Instance.DestroyAvatar(gameObject.name);
        print(gameObject.name);
    }
}
