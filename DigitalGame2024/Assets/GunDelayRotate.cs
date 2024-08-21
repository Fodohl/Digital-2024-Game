using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class GunDelayRotate : MonoBehaviour
{
    [SerializeField] private GameObject gunHolder;
    private Alteruna.Avatar avatar;
    private Quaternion originalRotation;
    private void Awake(){
        avatar = transform.parent.GetComponent<Alteruna.Avatar>();
        originalRotation = gunHolder.transform.localRotation;
    }
    void Update()
    {
        if (avatar.IsMe){
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            gunHolder.transform.localRotation = Quaternion.Lerp(gunHolder.transform.localRotation, Quaternion.Euler(new Vector3(originalRotation.eulerAngles.x + y * 5, originalRotation.eulerAngles.y + x * 5, originalRotation.eulerAngles.z)), 0.05f);
        }
    }
}
