using System.Collections;
using System.Collections.Generic;
using Alteruna;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : AttributesSync
{
    [SerializeField] private TextMeshProUGUI healthText; // UI text for displaying health
    [SerializeField] private GameObject[] UiPanels; // Array of UI panels for different game states
    [SerializeField] private GameObject scorePrefab; // Prefab for individual score entries
    [SerializeField] private GameObject scoreSection; // UI section for displaying scores
    private List<GameObject> scoreObjects = new List<GameObject>(); // List to manage score objects
    [SerializeField] private Color notAvaliableButtonColor; // Color for unavailable buttons
    [SerializeField] private Color notAvaliableTextColor; // Color for unavailable text
    [SerializeField] private Color avaliableButtonColor; // Color for available buttons
    [SerializeField] private Color avaliableTextColor; // Color for available text
    [SerializeField] private TextMeshProUGUI startButtonText; // UI text for the start button
    [SerializeField] private GameObject gameStartNotification; // Notification for game start countdown
    [SynchronizableField] private bool gameHasStarted; // Boolean to track if the game has started
    [SerializeField] private TextMeshProUGUI gameTimer; // UI text for the game timer
    private float timer = 300f; // Timer duration in seconds
    private static GameUIManager _instance; // Singleton instance

    // Singleton instance access
    public static GameUIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameUIManager>(); // Find existing instance
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GameUIManager");
                    _instance = singletonObject.AddComponent<GameUIManager>(); // Create new instance if none found
                }
            }
            return _instance; // Return the singleton instance
        }
    }

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
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
        // Check if there's an avatar and the game state is the start menu
        if (FindAnyObjectByType<Alteruna.Avatar>() && GameManager.Instance.gameState == GameManager.GameState.StartMenu)
        {
            StartCoroutine(SpawnAvatar()); // Start spawning avatars
        }

        // Manage start button text based on host status
        if (!Multiplayer.GetUser().IsHost && startButtonText.text != "Only the host can start the game")
        {
            startButtonText.text = "Only the host can start the game"; // Update button text
            startButtonText.color = notAvaliableTextColor; // Set text color
            startButtonText.transform.parent.GetComponent<Image>().color = notAvaliableButtonColor; // Set button color
            startButtonText.fontSize = 40; // Adjust font size
            startButtonText.transform.parent.GetComponent<Button>().interactable = false; // Disable button interaction
        }
        else if (Multiplayer.GetUser().IsHost && startButtonText.text != "Start Game")
        {
            startButtonText.text = "Start Game"; // Update button text
            startButtonText.color = avaliableTextColor; // Set text color
            startButtonText.transform.parent.GetComponent<Image>().color = avaliableButtonColor; // Set button color
            startButtonText.fontSize = 50; // Adjust font size
            startButtonText.transform.parent.GetComponent<Button>().interactable = true; // Enable button interaction
        }

        // Update timer if there's an avatar
        if (FindAnyObjectByType<Alteruna.Avatar>())
        {
            timer -= Time.deltaTime; // Decrement timer
            gameTimer.text = "Time: " + timer.ToString("F2"); // Update timer display
            if (timer <= 0)
            {
                SceneManager.LoadScene("Menu"); // Load menu scene when timer expires
            }
        }
    }

    // Updates the UI elements based on the game state
    public void UpdateUI()
    {
        UpdateTextElememts(); // Update text elements like health
        UpdateCursorState(); // Update cursor visibility and locking
        UpdateUIPanels(); // Update which UI panels are active
        UpdateScoreBoard(); // Update the scoreboard
    }

    // Updates health text based on current health of the avatar
    private void UpdateTextElememts()
    {
        if (GameManager.Instance.Multiplayer.GetAvatar() != null)
        {
            healthText.text = "Health: " + GameManager.Instance.Multiplayer.GetAvatar().GetComponent<Health>().currentHealth; // Display current health
        }
    }

    // Activates the appropriate UI panels based on the game state
    private void UpdateUIPanels()
    {
        UiPanels[0].SetActive(GameManager.Instance.gameState == GameManager.GameState.StartMenu); // Start menu panel
        UiPanels[1].SetActive(GameManager.Instance.gameState == GameManager.GameState.Playing); // Game playing panel
        UiPanels[2].SetActive(GameManager.Instance.gameState == GameManager.GameState.Paused); // Paused panel
        UiPanels[3].SetActive(GameManager.Instance.gameState == GameManager.GameState.ScoreBoard); // Scoreboard panel
    }

    // Updates cursor visibility and lock state based on game state
    private void UpdateCursorState()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.StartMenu || GameManager.Instance.gameState == GameManager.GameState.Paused)
        {
            Cursor.lockState = CursorLockMode.None; // Unlock cursor
            Cursor.visible = true; // Show cursor
        }
        if (GameManager.Instance.gameState == GameManager.GameState.Playing || GameManager.Instance.gameState == GameManager.GameState.ScoreBoard)
        {
            Cursor.lockState = CursorLockMode.Locked; // Lock cursor
            Cursor.visible = false; // Hide cursor
        }
    }

    // Updates the scoreboard with current player scores
    private void UpdateScoreBoard()
    {
        var team1 = 0; // Counter for team 1
        var team2 = 0; // Counter for team 2
        
        // Destroy existing score objects
        for (int i = 0; i < scoreObjects.Count; i++)
        {
            Destroy(scoreObjects[i]);
        }

        var users = Multiplayer.GetUsers(); // Get current users
        for (int i = 0; i < users.Count; i++)
        {
            // Check which team the user belongs to and instantiate score prefab accordingly
            if (GameManager.Instance.teams[0].Contains(users[i]))
            {
                var obj = Instantiate(scorePrefab, scoreSection.transform); // Instantiate score prefab
                obj.GetComponent<RectTransform>().localPosition = new Vector3(0, -((team1 * 50) - 275)); // Position score prefab
                scoreObjects.Add(obj); // Add to score object list
                obj.GetComponent<ScorePrefab>().name.text = users[i].Name; // Set player name
                obj.GetComponent<ScorePrefab>().kills.text = GameManager.Instance.kills[users[i]].ToString(); // Set kills
                obj.GetComponent<ScorePrefab>().deaths.text = GameManager.Instance.deaths[users[i]].ToString(); // Set deaths
                team1++; // Increment team 1 counter
            }
            else
            {
                var obj = Instantiate(scorePrefab, scoreSection.transform); // Instantiate score prefab
                obj.GetComponent<RectTransform>().localPosition = new Vector3(0, -((team2 * 50) + 25)); // Position score prefab
                scoreObjects.Add(obj); // Add to score object list
                obj.GetComponent<ScorePrefab>().name.text = users[i].Name; // Set player name
                obj.GetComponent<ScorePrefab>().kills.text = GameManager.Instance.kills[users[i]].ToString(); // Set kills
                obj.GetComponent<ScorePrefab>().deaths.text = GameManager.Instance.deaths[users[i]].ToString(); // Set deaths
                team2++; // Increment team 2 counter
            }
        }
    }

    // Coroutine to spawn avatar and manage game state
    public IEnumerator SpawnAvatar()
    {
        GameManager.Instance.gameState = GameManager.GameState.Playing; // Set game state to playing
        yield return new WaitForSeconds(3); // Wait before spawning

        // Spawn avatar at designated location based on dev mode
        if (GameManager.Instance.devSpawning)
        {
            Multiplayer.SpawnAvatar(new Vector3(292, 12, 544));
        }
        else
        {
            Multiplayer.SpawnAvatar(GameManager.Instance.teamSpawns[GameManager.Instance.GetCurrentTeam(Multiplayer.GetUser())]);
        }
        
        UpdateUI(); // Update the UI after spawning
    }

    // Broadcasts the start game request to all clients
    public void StartGame()
    {
        BroadcastRemoteMethod(nameof(StartGameIntermediate)); // Call intermediate start game method
    }

    // Method to handle game start logic across clients
    [SynchronizableMethod]
    private void StartGameIntermediate()
    {
        StartCoroutine(GameStartCountDown()); // Start countdown for game start
    }

    // Resumes the game when called
    public void ResumeGame()
    {
        GameManager.Instance.gameState = GameManager.GameState.Playing; // Set game state to playing
        UpdateUI(); // Update the UI
    }

    // Leaves the current room and loads the menu
    public void LeaveRoom()
    {
        Multiplayer.CurrentRoom.Leave(); // Leave the room
        Multiplayer.LoadScene("Menu"); // Load the menu scene
    }

    // Exits the application
    public void LeaveGame()
    {
        Application.Quit(); // Quit the application
    }

    // Coroutine for the game start countdown
    private IEnumerator GameStartCountDown()
    {
        GameManager.Instance.syncTeamsTemp(); // Sync teams temporarily
        gameStartNotification.SetActive(true); // Show game start notification
        if (GameManager.Instance.devSpawning)
        {
            yield return new WaitForSeconds(0); // No wait in dev mode
        }
        else
        {
            yield return new WaitForSeconds(5); // Wait for 5 seconds in normal mode
        }
        gameStartNotification.SetActive(false); // Hide game start notification
        StartCoroutine(SpawnAvatar()); // Spawn avatar after countdown
        gameHasStarted = true; // Mark the game as started
    }
}
