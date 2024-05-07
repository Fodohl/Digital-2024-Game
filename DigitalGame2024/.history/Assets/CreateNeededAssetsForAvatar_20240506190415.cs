using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController.Examples;
using UnityEngine;

public class CreateNeededAssetsForAvatar : MonoBehaviour
{
    public GameObject playerManager;
    public GameObject cameraManager;
    private GameObject setPlayerManager;
    // Start is called before the first frame update
    void Awake()
    {
        setPlayerManager = Instantiate(playerManager, new Vector3(0,0,0), Quaternion.identity);
        setPlayerManager.GetComponent<ExamplePlayer>().CharacterCamera = Instantiate(cameraManager, new Vector3(0,0,0), Quaternion.identity).GetComponent<ExampleCharacterCamera>();
        setPlayerManager.GetComponent<ExamplePlayer>().Character = gameObject.GetComponent<ExampleCharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
