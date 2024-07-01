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
    public void TakeHealth(float amount, User enemyUser){
        currentHealth -= amount;
        if (currentHealth <= 0f){
            Die(enemyUser);
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
    private void Die(User enemyUser){
        GameManager.Instance.
        GameManager.Instance.DestroyAvatar(gameObject.name);
        print(gameObject.name);
    }
}
