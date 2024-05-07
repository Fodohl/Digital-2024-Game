using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNeededAssetsForAvatar : MonoBehaviour
{
    public GameObject playerManager;
    // Start is called before the first frame update
    void Awake()
    {
        Instantiate(playerManager, new Vector3(0,0,0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
