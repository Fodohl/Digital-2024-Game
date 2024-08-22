using UnityEngine;
using Alteruna;
using System.Collections.Generic;
using System.Collections;

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
    [HideInInspector] public List<User>[] teams = new List<User>[] { new List<User>(10), new List<User>(10) };
    public Vector3[] teamSpawns = { new Vector3(100, 14, 550), new Vector3(1370, 15, 276) };
    public bool devSpawning = false;

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
        gameState = GameState.StartMenu;
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.P))
        {
            print(teams[0].Count);
            print(teams[1].Count);
        }
        if(Input.GetKeyDown(KeyCode.Tab)){
            gameState = GameState.ScoreBoard;
            GameUIManager.Instance.UpdateUI();
        }
        if(Input.GetKeyUp(KeyCode.Tab)){
            gameState = GameState.Playing;
            GameUIManager.Instance.UpdateUI();
        }
        if(Input.GetKeyDown(KeyCode.Escape) && gameState != GameState.Paused && gameState != GameState.StartMenu){
            gameState = GameState.Paused;
            GameUIManager.Instance.UpdateUI();
        }else if (Input.GetKeyDown(KeyCode.Escape) && gameState == GameState.Paused && gameState != GameState.StartMenu){
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
    public void OnOtherUserLeft(Multiplayer multiplayer, User user){
        teams[GetCurrentTeam(user)].RemoveAt(user);

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
    [SynchronizableMethod] public void DestroyAvatarBroadcast(string avatarName){
        var avatar = GameObject.Find(avatarName).GetComponent<Alteruna.Avatar>();
        AddDeaths(avatar.Possessor);
        if (avatar.IsMe){
            gameState = GameState.StartMenu;
            GameUIManager.Instance.UpdateUI();
            StartCoroutine(SpawnAfterSeconds(3));
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
        return teams[0].Count <= teams[1].Count ? 0 : 1;
    }
    public int GetCurrentTeam(User user){
        var tempTeam = -1;
        for (int i = 0; i < teams.Length; i++){
            if (teams[i].Contains(user)){
                tempTeam = i;
            }
        }
        return tempTeam;
    }
    public IEnumerator SpawnAfterSeconds(float seconds){
        yield return new WaitForSeconds(seconds);
        GameUIManager.Instance.SpawnAvatar();
    }
}