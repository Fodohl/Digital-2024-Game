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
        BroadcastRemoteMethod("UpdateUIBroadcast");
        if (currentHealth <= 0f){
            Die(enemyUser);
        }
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
        GameManager.Instance.AddKills(Multiplayer.GetUser());
        GameManager.Instance.DestroyAvatar(gameObject.GetComponent<Alteruna.Avatar>().Possessor, gameObject.name);
    }
}
