using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadScreenManager : MonoBehaviour
{
    private TMP_InputField inputField;
    private void Awake(){
        if (PlayerPrefs.HasKey("username")){
            SceneManager.LoadScene("Menu");
        }
    }
    public void Accept(){
        PlayerPrefs.SetString("username", inputField.text);
        SceneManager.LoadScene("Menu");
    }
}
