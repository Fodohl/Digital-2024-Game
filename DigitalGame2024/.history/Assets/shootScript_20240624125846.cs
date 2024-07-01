using System.Collections;
using System.Collections.Generic;
using Alteruna;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class shootScript : CommunicationBridge
{
    [SerializeField] private CustomGun currentGun;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (
                Physics.Raycast(transform.position, transform.forward, out hit)
                && transform.parent.GetComponent<Alteruna.Avatar>().IsMe
            )
            {
                if (
                    hit.transform.CompareTag("Player")
                    && hit.transform.gameObject != transform.parent.gameObject
                )
                {
                    hit.transform.gameObject.GetComponent<Health>().TakeHealth(20, transform.parent.gameObject.GetComponent<Alteruna.Avatar>().Possessor);
                }
            }
            StartCoroutine(shootTimer(currentGun.fireRate));
        }
    }
    private IEnumerator shootTimer(float fireRate){
        yield return new WaitForSeconds(1/fireRate);
    }
}