using UnityEngine;
using Alteruna;
using System.Collections.Generic;
using System.Collections;
using System;

public class GameManager : AttributesSync
{
    // Enum for different game states
    public enum GameState
    {
        StartMenu,
        Playing,
        Paused,
        ScoreBoard
    }

    [HideInInspector] public GameState gameState = GameState.StartMenu; // Current game state

    [SynchronizableField] public Dictionary<string, int> kills = new Dictionary<string, int>(); // Player kill counts
    [SynchronizableField] public Dictionary<string, int> deaths = new Dictionary<string, int>(); // Player death counts
    [HideInInspector] public List<User>[] teams = new List<User>[] { new List<User>(10), new List<User>(10) }; // Player teams
    public Vector3[] teamSpawns = { new Vector3(100, 14, 550), new Vector3(1370, 15, 276) }; // Spawn points for teams
    public bool devSpawning = false; // Flag for developer spawning mode

    private static GameManager _instance; // Singleton instance

    // Singleton instance access
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>(); // Find existing instance
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GameManager");
                    _instance = singletonObject.AddComponent<GameManager>(); // Create new instance if none found
                }
            }
            return _instance; // Return the singleton instance
        }
    }

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        gameState = GameState.StartMenu; // Initialize game state
        if (_instance == null)
        {
            _instance = this; // Set instance to this object
        }
        else if (_instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Debugging key: Print team sizes when 'P' is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            print(teams[0].Count);
            print(teams[1].Count);
        }

        // Toggle scoreboard state with 'Tab' key
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            gameState = GameState.ScoreBoard; // Set game state to scoreboard
            GameUIManager.Instance.UpdateUI(); // Update UI
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            gameState = GameState.Playing; // Reset game state to playing
            GameUIManager.Instance.UpdateUI(); // Update UI
        }

        // Pause and resume game with 'Escape' key
        if (Input.GetKeyDown(KeyCode.Escape) && gameState != GameState.Paused && gameState != GameState.StartMenu)
        {
            gameState = GameState.Paused; // Pause the game
            GameUIManager.Instance.UpdateUI(); // Update UI
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && gameState == GameState.Paused && gameState != GameState.StartMenu)
        {
            gameState = GameState.Playing; // Resume the game
            GameUIManager.Instance.UpdateUI(); // Update UI
        }
    }

    // Increments kill count for a user
    public void AddKills(User user)
    {
        kills[user.Name] += 1; // Increment the kill count for the specified user
    }

    // Increments death count for a user
    public void AddDeaths(User user)
    {
        deaths[user.Name] += 1; // Increment the death count for the specified user
        print(deaths[user.Name]); // Debugging: print the death count
    }

    // Requests to destroy an avatar
    public void DestroyAvatar(User user, string avatarName)
    {
        BroadcastRemoteMethod("DestroyAvatarBroadcast", avatarName); // Broadcast method to destroy avatar
    }

    // Called when another user joins the game
    public void OnOtherUserJoined(Multiplayer multiplayer, User user)
    {
        // Initialize kill and death counts for new users
        if (!kills.ContainsKey(user.Name))
        {
            kills.Add(user.Name, 0); // Add user to kills dictionary
        }
        if (!deaths.ContainsKey(user.Name))
        {
            deaths.Add(user.Name, 0); // Add user to deaths dictionary
        }

        // Check if user is already in a team
        var isInTeam = false;
        for (int i = 0; i < teams.Length; i++)
        {
            if (teams[i].Contains(user))
            {
                isInTeam = true; // User is already in a team
            }
        }

        // If not in a team, add to the smallest team
        if (!isInTeam)
        {
            teams[GetSmallestTeam()].Add(user);
        }
    }

    // Called when a user leaves the game
    public void OnOtherUserLeft(Multiplayer multiplayer, User user)
    {
        teams[GetCurrentTeam(user)].Remove(user); // Remove user from their current team
    }

    // Called when a room is joined
    public void OnRoomJoined(Multiplayer multiplayer, Room room, User user)
    {
        // Initialize kill and death counts for new users
        if (!kills.ContainsKey(user.Name))
        {
            kills.Add(user.Name, 0); // Add user to kills dictionary
        }
        if (!deaths.ContainsKey(user.Name))
        {
            deaths.Add(user.Name, 0); // Add user to deaths dictionary
        }

        // Check if user is already in a team
        var isInTeam = false;
        for (int i = 0; i < teams.Length; i++)
        {
            if (teams[i].Contains(user))
            {
                isInTeam = true; // User is already in a team
            }
        }

        // If not in a team, add to the smallest team
        if (!isInTeam)
        {
            teams[GetSmallestTeam()].Add(user);
        }
    }

    // Method to broadcast avatar destruction to all clients
    [SynchronizableMethod]
    public void DestroyAvatarBroadcast(string avatarName)
    {
        var avatar = GameObject.Find(avatarName).GetComponent<Alteruna.Avatar>(); // Find the avatar by name
        AddDeaths(avatar.Possessor); // Increment death count for the avatar possessor

        // Check if the avatar is controlled by the local user
        if (avatar.IsMe)
        {
            gameState = GameState.StartMenu; // Set game state to start menu
            GameUIManager.Instance.UpdateUI(); // Update UI
            StartCoroutine(SpawnAfterSeconds(3)); // Start coroutine to respawn after a delay
        }
        Destroy(avatar.gameObject); // Destroy the avatar
    }

    // Set the user's team
    [SynchronizableMethod]
    public void SetUsersTeam(User user, int team)
    {
        var i = team == 0 ? 1 : 0; // Get the opposite team index
        if (teams[i].Contains(user))
        {
            teams[i].Remove(user); // Remove user from the opposite team if present
        }
        if (!teams[team].Contains(user))
        {
            teams[team].Add(user); // Add user to the specified team
        }
    }

    // Get the index of the team with fewer players
    private int GetSmallestTeam()
    {
        return teams[0].Count <= teams[1].Count ? 0 : 1; // Return index of the smaller team
    }

    // Get the current team of a user
    public int GetCurrentTeam(User user)
    {
        var tempTeam = -1; // Initialize team index
        for (int i = 0; i < teams.Length; i++)
        {
            if (teams[i].Contains(user))
            {
                tempTeam = i; // Set team index if user is found
            }
        }
        return tempTeam; // Return the team index
    }

    // Coroutine to respawn the avatar after a specified delay
    public IEnumerator SpawnAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds); // Wait for the specified time
        GameUIManager.Instance.SpawnAvatar(); // Spawn the avatar
    }

    // Sync teams temporarily across clients
    public void syncTeamsTemp()
    {
        BroadcastRemoteMethod(nameof(syncTeams)); // Broadcast sync teams method
    }

    // Synchronizes team information across clients
    [SynchronizableMethod]
    public void syncTeams()
    {
        var team = teams; // Capture current teams
        teams = team; // Update teams (this might be redundant; consider removing)
    }
}
