using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
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
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void UpdateUI(){
        healthText.text = "Health: " + GameManager.Instance.Multiplayer.GetAvatar();
    }
}
