using System.Collections;
using System.Collections.Generic;
using Alteruna;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameUIManager : CommunicationBridge
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject[] UiPanels;
    [SerializeField] private GameObject scorePrefab;
    [SerializeField] private GameObject scoreSection;
    private List<GameObject> scoreObjects = new List<GameObject>();
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
    public void UpdateUI(){
        UpdateTextElememts();
        UpdateCursorState();
        UpdateUIPanels();
    }
    private void UpdateTextElememts(){
        healthText.text = "Health: " + GameManager.Instance.Multiplayer.GetAvatar().GetComponent<Health>().currentHealth;
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
        for (int i = 0; i < scoreObjects.Count; i++)
        {
            Destroy(scoreObjects[i]); 
        }
        for (int i = 0; i < GameManager.Instance.kills.Count; i++)
        {
        //     for (int j = 0; j < scoreObjects.Count; j++)
        //     {
        //         if (Multiplayer.GetUser(scoreObjects[j].GetComponent<ScorePrefab>().name.text) == Multiplayer.GetUser(i)){
                    var obj = Instantiate(scorePrefab, scoreSection.transform);
                    obj.GetComponent<RectTransform>().position = new Vector3(0, (i*50) + 25);
                    scoreObjects.Add(obj);
                    obj.GetComponent<ScorePrefab>().name.text = Multiplayer.GetUser(i).Name;
                    obj.GetComponent<ScorePrefab>().kills.text = GameManager.Instance.kills[i].ToString();
                    obj.GetComponent<ScorePrefab>().deaths.text = GameManager.Instance.deaths[i].ToString();
        //         }
        //     }
        }
    }
    public void SpawnAvatar(){
        GameManager.Instance.SpawnAvatar();
    }
}
