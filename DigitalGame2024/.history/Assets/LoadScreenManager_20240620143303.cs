using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScreenManager : MonoBehaviour
{
    private void Awake(){
        if (PlayerPrefs.GetString("username") != null){
            SceneManager.LoadScene("Menu");
        }
    }
}
