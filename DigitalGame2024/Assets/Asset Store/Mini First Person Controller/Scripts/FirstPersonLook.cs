﻿using UnityEngine;
using Alteruna;

public class FirstPersonLook : MonoBehaviour
{
    public float sensitivity = 2;
    public float smoothing = 1.5f;

    Vector2 velocity;
    Vector2 frameVelocity;
    private Alteruna.Avatar avatar;
    [SerializeField] private Transform parent;


    private void Awake(){
        avatar = parent.GetComponent<Alteruna.Avatar>();
    }
    void Start()
    {
        // Lock the mouse cursor to the game screen.
        if (avatar.IsMe){
            Cursor.lockState = CursorLockMode.Locked;
        }
    }


    void Update()
    {
        if (avatar.IsMe){
            if (GameManager.Instance.gameState == GameManager.GameState.Playing || GameManager.Instance.gameState == GameManager.GameState.ScoreBoard){
                // Get smooth velocity.
                Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
                frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
                velocity += frameVelocity;
                velocity.y = Mathf.Clamp(velocity.y, -90, 90);

                // Rotate camera up-down and controller left-right from velocity.
                transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
                parent.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
            }
        }
    }
}
