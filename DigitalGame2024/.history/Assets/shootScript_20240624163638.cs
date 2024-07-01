using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;

public class shootScript : CommunicationBridge
{
    [SerializeField] private CustomGun currentGun;
    [SerializeField] private Transform parent;
    private bool canShoot = true;
    private int currentAmmo;
    private void Awake(){
        currentAmmo = currentGun.magSize;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (
                Physics.Raycast(transform.position, transform.forward, out hit)
                && parent.GetComponent<Alteruna.Avatar>().IsMe
            )
            {
                if (
                    hit.transform.CompareTag("Player")
                    && hit.transform.gameObject != parent.gameObject
                )
                {
                    hit.transform.gameObject.GetComponent<Health>().TakeHealth(currentGun.damage, parent.gameObject.GetComponent<Alteruna.Avatar>().Possessor);
                }
            }
            currentAmmo--;
            canShoot = false;
            StartCoroutine(shootTimer(currentGun.fireRate));
        }
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < currentGun.magSize){
            canShoot = false;
            StartCoroutine(reloadTimer(currentGun.reloadTime));
        }
    }
    private IEnumerator shootTimer(float fireRate){
        yield return new WaitForSeconds(1/fireRate);
        canShoot = true;
    }
    private IEnumerator reloadTimer(float reloadTime){
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = currentGun.magSize;
        canShoot = true;
    }
}
