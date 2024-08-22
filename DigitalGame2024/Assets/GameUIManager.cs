using System.Collections;
using System.Collections.Generic;
using Alteruna;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : AttributesSync
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject[] UiPanels;
    [SerializeField] private GameObject scorePrefab;
    [SerializeField] private GameObject scoreSection;
    private List<GameObject> scoreObjects = new List<GameObject>();
    [SerializeField] private Color notAvaliableButtonColor;
    [SerializeField] private Color notAvaliableTextColor;
    [SerializeField] private Color avaliableButtonColor;
    [SerializeField] private Color avaliableTextColor;
    [SerializeField] private TextMeshProUGUI startButtonText;
    [SerializeField] private GameObject gameStartNotification;
    [SynchronizableField] private bool gameHasStarted;
    private static GameUIManager _instance;
    public static GameUIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameUIManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GameUIManager");
                    _instance = singletonObject.AddComponent<GameUIManager>();
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
    private void Update(){
        if (GameObject.FindAnyObjectByType<Alteruna.Avatar>() && GameManager.Instance.gameState == GameManager.GameState.StartMenu){
            StartCoroutine(SpawnAvatar());
        }
        if (!Multiplayer.GetUser().IsHost && startButtonText.text != "Only the host can start the game"){
            startButtonText.text = "Only the host can start the game";
            startButtonText.color = notAvaliableTextColor;
            startButtonText.transform.parent.GetComponent<Image>().color = notAvaliableButtonColor;
            startButtonText.fontSize = 40;
            startButtonText.transform.parent.GetComponent<Button>().interactable = false;
        }else if (Multiplayer.GetUser().IsHost && startButtonText.text != "Start Game"){
            startButtonText.text = "Start Game";
            startButtonText.color = avaliableTextColor;
            startButtonText.transform.parent.GetComponent<Image>().color = avaliableButtonColor;
            startButtonText.fontSize = 50;
            startButtonText.transform.parent.GetComponent<Button>().interactable = true;
        }
    }
    public void UpdateUI(){
        UpdateTextElememts();
        UpdateCursorState();
        UpdateUIPanels();
        UpdateScoreBoard();
    }
    private void UpdateTextElememts(){
        if (GameManager.Instance.Multiplayer.GetAvatar() != null){
            healthText.text = "Health: " + GameManager.Instance.Multiplayer.GetAvatar().GetComponent<Health>().currentHealth;
        }
    }
    private void UpdateUIPanels(){
        UiPanels[0].SetActive(GameManager.Instance.gameState == GameManager.GameState.StartMenu);
        UiPanels[1].SetActive(GameManager.Instance.gameState == GameManager.GameState.Playing);
        UiPanels[2].SetActive(GameManager.Instance.gameState == GameManager.GameState.Paused);
        UiPanels[3].SetActive(GameManager.Instance.gameState == GameManager.GameState.ScoreBoard);
    }
    private void UpdateCursorState(){
         if(GameManager.Instance.gameState == GameManager.GameState.StartMenu || GameManager.Instance.gameState == GameManager.GameState.Paused){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if(GameManager.Instance.gameState == GameManager.GameState.Playing || GameManager.Instance.gameState == GameManager.GameState.ScoreBoard){
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void UpdateScoreBoard(){
        var team1 = 0;
        var team2 = 0;
        for (int i = 0; i < scoreObjects.Count; i++)
        {
            Destroy(scoreObjects[i]); 
        }
        var users = Multiplayer.GetUsers();
        for (int i = 0; i < users.Count; i++)
        {
            if(GameManager.Instance.teams[0].Contains(users[i])){
                var obj = Instantiate(scorePrefab, scoreSection.transform);
                obj.GetComponent<RectTransform>().localPosition = new Vector3(0, -((team1*50) - 275));
                scoreObjects.Add(obj);
                obj.GetComponent<ScorePrefab>().name.text = users[i].Name;
                obj.GetComponent<ScorePrefab>().kills.text = GameManager.Instance.kills[users[i]].ToString();
                obj.GetComponent<ScorePrefab>().deaths.text = GameManager.Instance.deaths[users[i]].ToString();
                team1++;
            }else{
                var obj = Instantiate(scorePrefab, scoreSection.transform);
                obj.GetComponent<RectTransform>().localPosition = new Vector3(0, -((team2*50) + 25));
                scoreObjects.Add(obj);
                obj.GetComponent<ScorePrefab>().name.text = users[i].Name;
                obj.GetComponent<ScorePrefab>().kills.text = GameManager.Instance.kills[users[i]].ToString();
                obj.GetComponent<ScorePrefab>().deaths.text = GameManager.Instance.deaths[users[i]].ToString();
                team2++;
            }
        }
    }
    public IEnumerator SpawnAvatar(){
        GameManager.Instance.gameState = GameManager.GameState.Playing;
        yield return new WaitForSeconds(3);
        if (GameManager.Instance.devSpawning){
            Multiplayer.SpawnAvatar(new Vector3(292, 12, 544));
        }else{
            Multiplayer.SpawnAvatar(GameManager.Instance.teamSpawns[GameManager.Instance.GetCurrentTeam(Multiplayer.GetUser())]);
        }
        UpdateUI();
    }
    public void StartGame(){
        BroadcastRemoteMethod(nameof(StartGameIntermediate));
    }
    [SynchronizableMethod]
    private void StartGameIntermediate(){
        StartCoroutine(GameStartCountDown());
    }
    public void ResumeGame(){
        GameManager.Instance.gameState = GameManager.GameState.Playing;
        UpdateUI();
    }
    public void LeaveRoom(){
        Multiplayer.CurrentRoom.Leave();
        Multiplayer.LoadScene("Menu");
    }
    public void LeaveGame(){
        Application.Quit();
    }
    private IEnumerator GameStartCountDown(){
        gameStartNotification.SetActive(true);
        if (GameManager.Instance.devSpawning){
            yield return new WaitForSeconds(0);
        }else{
            yield return new WaitForSeconds(5);
        }
        gameStartNotification.SetActive(false);
        StartCoroutine(SpawnAvatar());
        gameHasStarted = true;
    }
}
