using System;
using TMPro;
using UnityEngine;
using Alteruna;
using System.Collections.Generic;
using System.Data;

public class GameManager : AttributesSync
{
    public enum GameState{
        StartMenu,
        Playing,
        Paused,
        ScoreBoard
    }
    [HideInInspector] public GameState gameState = GameState.StartMenu;

    [SynchronizableField] public Dictionary< string, int> kills = new Dictionary<string, int>();
    [SynchronizableField] public Dictionary< string, int> deaths = new Dictionary<string, int>();
    private List<User>[] teams;

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
        kills[user.Name] += 1;
    }
    public void AddDeaths(User user){
        deaths[user.Name] += 1;
        print(deaths[user.Name]);
    }
    public void DestroyAvatar(User user, string avatarName){
        BroadcastRemoteMethod("DestroyAvatarBroadcast", avatarName);
    }
    public void OnOtherUserJoined(Multiplayer multiplayer, User user){
        if(!kills.ContainsKey(user.Name))
        {
            kills.Add(user.Name, 0);
        }
        if(!deaths.ContainsKey(user.Name))
        {
            deaths.Add(user.Name, 0);
        }
        var isInTeam = false;
        for (int i = 0; i < teams.Length; i++)
        {
            if (teams[i].Contains(user)){
                isInTeam = true;
            }
        }
        if (!isInTeam){
            teams[GetSmallestTeam()].Add(user);
        }
    }
    public void OnRoomJoined(Multiplayer multiplayer, Room room, User user){
        if(!kills.ContainsKey(user.Name))
        {
            kills.Add(user.Name, 0);
        }
        if(!deaths.ContainsKey(user.Name))
        {
            deaths.Add(user.Name, 0);
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
    [SynchronizableMethod] public void SetUsersTeam(User user, int team){
        var i = team == 0 ? 1 : 0;
        if (teams[i].Contains(user)){
            teams[i].Remove(user);
        }
        if (!teams[team].Contains(user)){
            teams[team].Add(user);
        }
    }
    private int GetSmallestTeam(){
        return teams[0].Count < teams[1].Count ? 0 : 1;
    }
    private int GetCurrentTeam(User user){
        var tempTeam = -1;
        for (int i = 0; i < teams.Length; i++){
            if (teams[i].Contains(user)){
                tempTeam = i;
            }
        }
        return tempTeam;
    }
}