using System;
using TMPro;
using UnityEngine;
using Alteruna;
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

    [SynchronizableField] public Dictionary< int, int> kills = new Dictionary<int, int>();
    [SynchronizableField] public Dictionary< int, int> deaths = new Dictionary<int, int>();

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
    private void Update(){
        if (Input.GetKeyDown(KeyCode.P)){
            print(kills[0]);
            print(deaths[0]);
        }
        if(Input.GetKeyDown(KeyCode.Tab)){
            gameState = GameState.ScoreBoard;
            GameUIManager.Instance.UpdateUI();
        }
        if(Input.GetKeyUp(KeyCode.Tab)){
            gameState = GameState.Playing;
            GameUIManager.Instance.UpdateUI();
        }
    }
    public void AddKills(User user){
        kills[user.Index] += 1;
    }
    public void AddDeaths(User user){
        deaths[user.Index] += 1;
        print(deaths[user.Index]);
    }
    public void DestroyAvatar(User user, string avatarName){
        BroadcastRemoteMethod("DestroyAvatarBroadcast", avatarName);
    }
    public void OnOtherUserJoined(Multiplayer multiplayer, User user){
        if(!kills.ContainsKey(user.Index))
        {
            kills.Add(user.Index, 0);
        }
        if(!deaths.ContainsKey(user.Index))
        {
            deaths.Add(user.Index, 0);
        }
    }
    public void OnRoomJoined(Multiplayer multiplayer, Room room, User user){
        if(!kills.ContainsKey(user.Index))
        {
            kills.Add(user.Index, 0);
        }
        if(!deaths.ContainsKey(user.Index))
        {
            deaths.Add(user.Index, 0);
        }
    }
    [SynchronizableMethod] public void DestroyAvatarBroadcast(string avatarName){
        var avatar = GameObject.Find(avatarName).GetComponent<Alteruna.Avatar>();
        AddDeaths(avatar.Possessor);
        if (avatar.IsMe){
            gameState = GameState.StartMenu;
            GameUIManager.Instance.UpdateUI();
        }
        Destroy(avatar.gameObject);
    }

}