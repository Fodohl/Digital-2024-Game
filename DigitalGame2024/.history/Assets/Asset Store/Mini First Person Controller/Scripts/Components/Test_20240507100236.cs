using Alteruna;
using System;
using UnityEngine;

public class InteractablePlayer : AttributesSync, IInteractable
{
    public bool tagged = false; // Boolean to track if the prey is tagged

    public Vector3 prisonPosition = new Vector3(63.7f, 10.58f, -17.28f);
    public Alteruna.Avatar _avatar;
    private string myName;
    private bool GotTagged;

    //Temp Variables
    []private Gameobject interactor;
    private Alteruna.Avatar interactorAvatar;
    public void Start()
    {
        myName = gameObject.name;
        GotTagged = false;
    }

    private void Update()
    {
        if (GotTagged)
        {
            //GetTP();

        }
    }


    public GameObject GiveObject()
    {
        return gameObject;
    }
    public void InitInteract(GameObject Iinteractor, Alteruna.Avatar IinteractorAvatar){
        if (_avatar.IsMe){
            interactor = Iinteractor;
            interactorAvatar = IinteractorAvatar;
            BroadcastRemoteMethod("Interact");
        }
    }

    [SynchronizableMethod] 
    public void Interact()
    {
        // Check if both the interactor and the player are on specific layers
        if (interactor.layer == LayerMask.NameToLayer("Hunter") && gameObject.layer == LayerMask.NameToLayer("Prey"))
        {
            // Teleport the player to the prison position
            GetComponent<TransformSynchronizable>().transform.position = prisonPosition;
            GotTagged = true;
        }
    }

    //private void GetTP()
    //{
    //    GetComponent<TransformSynchronizable>().transform.position = prisonPosition;
    //}
}