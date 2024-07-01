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
    private void Update(){
        if (currentHealth <= 0f){
            Die();
        }
    }
    public void TakeHealth(float amount){
        currentHealth -= amount;
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
        GameManager.Instance.DestroyAvatar(gameObject.GetInstanceID());
        print(gameObject.GetInstanceID());
    }
}
