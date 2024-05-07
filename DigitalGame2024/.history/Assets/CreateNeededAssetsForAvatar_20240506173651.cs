using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController.Examples;
using UnityEngine;

public class CreateNeededAssetsForAvatar : MonoBehaviour
{
    public GameObject playerManager;
    private GameObject setPlayerManager;
    // Start is called before the first frame update
    void Awake()
    {
        setPlayerManager = Instantiate(playerManager, new Vector3(0,0,0), Quaternion.identity);
        setPlayerManager.GetComponent<ExamplePlayer>().Character = this.gameObject.GetComponent<ExampleCharacterController>();
        setPlayerManager.GetComponent<ExamplePlayer>().CharacterCamera = this.gameObject.GetComponent<ExampleCharacterCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
