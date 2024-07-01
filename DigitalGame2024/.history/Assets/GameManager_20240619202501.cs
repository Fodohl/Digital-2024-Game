using System;
using TMPro;
using UnityEngine;
using Alteruna;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : AttributesSync
{
    [SerializeField] private references references;
    public enum GameState{
        StartMenu,
        Playing,
        Paused,
        ScoreBoard
    }
    public GameState gameState = GameState.StartMenu;

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
    }

    public void DestroyAvatar(int avatarID, Alteruna.Avatar avatar){
        if (avatar.IsMe){
            gameState = GameState.StartMenu;
        }
        BroadcastRemoteMethod("DestroyAvatarBroadcast", avatarID);
    }
    [SynchronizableMethod]
    public void DestroyAvatarBroadcast(int avatarID){
        Destroy(GameObject.Find(avatarID.ToString()));
    }
}