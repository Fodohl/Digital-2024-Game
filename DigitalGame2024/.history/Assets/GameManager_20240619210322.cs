using System;
using TMPro;
using UnityEngine;
using Alteruna;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : AttributesSync
{
    public enum GameState{
        StartMenu,
        Playing,
        Paused,
        ScoreBoard
    }
    [HideInInspector] public GameState gameState = GameState.StartMenu;

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

    public void SpawnAvatar(){
        Multiplayer.SpawnAvatar(new Vector3(0, 10, 0));
        gameState = GameState.Playing;
        GameUIManager.Instance.UpdateUI();
    }

    public void DestroyAvatar(int avatarID){

        BroadcastRemoteMethod("DestroyAvatarBroadcast", avatarID);
    }

    [SynchronizableMethod]
    public void DestroyAvatarBroadcast(int avatarID){
        if (GameObject.Find(avatarID.ToString()).GetComponent<Alteruna.Avatar>().IsMe){
            gameState = GameState.StartMenu;
        }
        Destroy(GameObject.Find(avatarID.ToString()));
    }
}