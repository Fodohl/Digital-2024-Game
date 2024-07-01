using System;
using TMPro;
using UnityEngine;
using Alteruna;
using UnityEngine.SceneManagement;

public class GameManager : AttributesSync
{
    [SerializeField] private GameObject[] spawnLocationTeamA;
    [SerializeField] private GameObject[] spawnLocationTeamB;
    [SerializeField] private references references;
    [SerializeField] private bool inRoom = false;
    [SerializeField] private Health health;
    public TextMeshProUGUI healthText;

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    private void Update()
    {
        if (inRoom)
        {
            if (Multiplayer.InRoom)
            {
                if (healthText == null)
                {
                    healthText = FindObjectOfType<references>().GetComponent<references>().healthText;
                }
                if (health == null)
                {
                    var healthScripts = FindObjectsOfType<Health>();
                    foreach (var healthScript in healthScripts)
                    {
                        if (healthScript.gameObject.GetComponent<Alteruna.Avatar>().IsMe)
                        {
                            health = healthScript;
                        }
                    }
                }
                else
                {
                    healthText.text = "Health: " + health.currentHealth;
                }
                if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Game") && references == null)
                {
                    references = FindObjectOfType<references>().GetComponent<references>();
                }
            }
        }
    }

    public void OnRoomJoined(Multiplayer multiplayerInstance, Room room, User user)
    {
        inRoom = true;
    }

    public void SpawnAvatar()
    {
        Multiplayer.SpawnAvatar(new Vector3(0, 10, 0));
        references.uiElememts[0].SetActive(false);
        references.uiElememts[1].SetActive(true);
    }

    public void DestroyAvatar(string avatarID)
    {
        references.uiElememts[1].SetActive(false);
        references.uiElememts[0].SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        BroadcastRemoteMethod("DestroyAvatarBroadcast", avatarID);
    }

    [SynchronizableMethod]
    public void DestroyAvatarBroadcast(string avatarID)
    {
        // Find the avatar by ID and destroy it
        var avatars = FindObjectsOfType<Health>();
        foreach (var avatar in avatars)
        {
            if (avatar.avatarID == avatarID)
            {
                Destroy(avatar.gameObject);
                break;
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void BroadcastDeath(string avatarID)
    {
        DestroyAvatar(avatarID);
    }
}
