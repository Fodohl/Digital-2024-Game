using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject[] UiPanels;
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
        print("updated ui");
        healthText.text = "Health: " + GameManager.Instance.Multiplayer.GetAvatar().gameObject.GetComponent<Health>().currentHealth;
        UiPanels[0].SetActive(GameManager.Instance.gameState == GameManager.GameState.StartMenu); print(GameManager.Instance.gameState == GameManager.GameState.StartMenu);
        UiPanels[1].SetActive(GameManager.Instance.gameState == GameManager.GameState.Playing);
        UiPanels[2].SetActive(GameManager.Instance.gameState == GameManager.GameState.Paused);
        UiPanels[3].SetActive(GameManager.Instance.gameState == GameManager.GameState.ScoreBoard);
        if(GameManager.Instance.gameState == GameManager.GameState.StartMenu || GameManager.Instance.gameState == GameManager.GameState.Paused){
            Cursor.lockState = CursorLockMode.None;
        }
        if(GameManager.Instance.gameState == GameManager.GameState.Playing || GameManager.Instance.gameState == GameManager.GameState.ScoreBoard){
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
