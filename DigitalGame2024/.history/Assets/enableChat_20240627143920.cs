using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enableChat : MonoBehaviour
{
    [SerializeField]private InputField input;
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return)){
            var setActive = input.gameObject.activeInHierarchy;
            if (setActive){
                input.gameObject.SetActive(false);
            }else{
                input.gameObject.SetActive(true);
                input.ActivateInputField();
            } 
        }
    }
}
