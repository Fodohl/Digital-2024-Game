using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject[] UiPanels;
    [SerializeField] private GameObject scorePrefab;
    [SerializeField] private GameObject scoreSection;
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
        UiPanels[3].
    }
    public void SpawnAvatar(){
        GameManager.Instance.SpawnAvatar();
    }
}
