using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class shootScript : MonoBehaviour
{
    private void Update(){
        if (Input.GetMouseButtonDown(0)){
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit) && transform.parent.GetComponent<Alteruna.Avatar>().IsMe){
                if (hit.transform.CompareTag("Player") && hit.transform.gameObject != transform.parent.gameObject){
                    hit.transform.gameObject.GetComponent<Health>().TakeHealth(20);
                }
            }
        }
    }
}
