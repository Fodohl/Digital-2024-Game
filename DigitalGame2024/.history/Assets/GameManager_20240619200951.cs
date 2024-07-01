using System;
using TMPro;
using UnityEngine;
using Alteruna;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : AttributesSync
{
    [SerializeField] private GameObject[] spawnLocationTeamA;
    [SerializeField] private GameObject[] spawnLocationTeamB;
    [SerializeField] private references references;
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
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GameManager");
                    _instance = singletonObject.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (Multiplayer.InRoom && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Game"))
        {
        //     if (healthText == null){
        //         healthText = FindObjectOfType<references>().GetComponent<references>().healthText;
        //     }
        //     if (health == null)
        //     {
        //         var healthScripts = FindObjectsOfType<Health>();
        //         foreach (var healthScript in healthScripts)
        //         {
        //             if (healthScript.gameObject.GetComponent<Alteruna.Avatar>().IsMe)
        //             {
        //                 health = healthScript;
        //             }
        //         }
        //     }
        //     else
        //     {
        //         healthText.text = "Health: " + health.currentHealth;
        //     }
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Game") && references == null){
                references = FindObjectOfType<references>().GetComponent<references>();
            }
        }
    }
    public void SpawnAvatar(){
        Multiplayer.SpawnAvatar(new Vector3(0, 10, 0));
        references.uiElememts[0].SetActive(false);
        references.uiElememts[1].SetActive(true);
    }

    public void DestroyAvatar(int avatarID, Alteruna.Avatar avatar){
        if (avatar.IsMe){
            references.uiElememts[1].SetActive(false);
            references.uiElememts[0].SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        BroadcastRemoteMethod("DestroyAvatarBroadcast", avatarID);
    }
    [SynchronizableMethod]
    public void DestroyAvatarBroadcast(int avatarID){
        Destroy(GameObject.Find(avatarID.ToString()));
    }
}