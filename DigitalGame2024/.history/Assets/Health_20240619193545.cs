using UnityEngine;
using Alteruna;

public class Health : AttributesSync
{
    public int maxHealth = 100;
    public int currentHealth;
    public string avatarID;

    private void Start()
    {
        currentHealth = maxHealth;
        // Ensure the avatarID is unique
        avatarID = System.Guid.NewGuid().ToString();
        SyncID(avatarID);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }

        // Sync the health with all clients
        SyncHealth(currentHealth);
    }

    [SynchronizableMethod]
    void SyncHealth(int newHealth)
    {
        currentHealth = newHealth;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Broadcast the death event with the avatar's ID
        FindObjectOfType<GameManager>().DestroyAvatar(avatarID);
    }

    [SynchronizableMethod]
    void SyncID(string id)
    {
        avatarID = id;
    }
}
