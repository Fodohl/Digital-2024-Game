using System.Collections;
using System.Collections.Generic;
using Alteruna;
using Unity.VisualScripting;
using UnityEngine;

public class shootScript : CommunicationBridge
{
    [SerializeField] private CustomGun[] allGuns;
    [SerializeField] private GameObject[] gunGameObjects;
    [SerializeField] private List<int> ammoInEachGun = new List<int>();
    private CustomGun currentGun;
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject bloodSplatterEffect;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawnPlace;
    private bool canShoot = true;
    private GameObject holdFlash;
    private int currentAmmo;
    [SerializeField] private GameObject[] muzzelFlash;
    [SerializeField] private GameObject muzzelSpawn;
    private void Awake(){
        currentGun = allGuns[0];
        currentAmmo = currentGun.magSize;
        for (int i = 0; i < allGuns.Length; i++)
        {
            ammoInEachGun[i] = allGuns[i].magSize;
        }
    }
    private void Update()
    {
        if (currentGun.gunType == CustomGun.fireType.singleFire){
            if (Input.GetMouseButtonDown(0) && canShoot && currentAmmo > 0 && transform.parent.GetComponent<Alteruna.Avatar>().IsMe)
            {
                for (int i = 0; i < transform.GetChild(0).childCount; i++){
                    if (transform.GetChild(0).GetChild(i).gameObject.activeSelf){
                        transform.GetChild(0).GetChild(i).GetComponent<Animator>().SetTrigger("Shoot");
                    }
                }
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
                        if (GameManager.Instance.GetCurrentTeam(hit.transform.gameObject.GetComponent<Alteruna.Avatar>().Owner) != GameManager.Instance.GetCurrentTeam(Multiplayer.GetUser())){
                            hit.transform.gameObject.GetComponent<Health>().TakeHealth(currentGun.damage, parent.gameObject.GetComponent<Alteruna.Avatar>().Possessor);
                            Instantiate(bloodSplatterEffect, hit.point, Quaternion.identity);
                        }
                    }
                }
                int randomNumberForMuzzelFlash = Random.Range(0,5);
                    if (bullet){
                        GameObject x = Instantiate(bullet, bulletSpawnPlace.transform.position, bulletSpawnPlace.transform.rotation);
                        x.transform.LookAt(hit.point);
                    }
                    holdFlash = Instantiate(muzzelFlash[randomNumberForMuzzelFlash], muzzelSpawn.transform.position /*- muzzelPosition*/, muzzelSpawn.transform.rotation * Quaternion.Euler(0,0,90) ) as GameObject;
                    holdFlash.transform.parent = muzzelSpawn.transform;
                currentAmmo--;
                canShoot = false;
                StartCoroutine(shootTimer(currentGun.fireRate));
            }
        }else{
            if (Input.GetMouseButton(0) && canShoot && currentAmmo > 0 && transform.parent.GetComponent<Alteruna.Avatar>().IsMe)
            {
                for (int i = 0; i < transform.GetChild(0).childCount; i++){
                    if (transform.GetChild(0).GetChild(i).gameObject.activeSelf){
                        transform.GetChild(0).GetChild(i).GetComponent<Animator>().SetTrigger("Shoot");
                    }
                }
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
                        if (GameManager.Instance.GetCurrentTeam(hit.transform.gameObject.GetComponent<Alteruna.Avatar>().Owner) != GameManager.Instance.GetCurrentTeam(Multiplayer.GetUser())){
                            hit.transform.gameObject.GetComponent<Health>().TakeHealth(currentGun.damage, parent.gameObject.GetComponent<Alteruna.Avatar>().Possessor);
                            Instantiate(bloodSplatterEffect, hit.point, Quaternion.identity);
                        }
                    }
                }
                int randomNumberForMuzzelFlash = Random.Range(0,5);
                    if (bullet){
                        GameObject x = Instantiate(bullet, bulletSpawnPlace.transform.position, bulletSpawnPlace.transform.rotation);
                        x.transform.LookAt(hit.point);
                    }
                    holdFlash = Instantiate(muzzelFlash[randomNumberForMuzzelFlash], muzzelSpawn.transform.position /*- muzzelPosition*/, muzzelSpawn.transform.rotation * Quaternion.Euler(0,0,90) ) as GameObject;
                    holdFlash.transform.parent = muzzelSpawn.transform;
                currentAmmo--;
                canShoot = false;
                StartCoroutine(shootTimer(currentGun.fireRate));
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < currentGun.magSize && transform.parent.GetComponent<Alteruna.Avatar>().IsMe){
            canShoot = false;
            for (int i = 0; i < transform.GetChild(0).childCount; i++)
            {
                if (transform.GetChild(0).GetChild(i).gameObject.activeSelf){
                    transform.GetChild(0).GetChild(i).GetComponent<Animator>().SetTrigger("Reload");
                    StartCoroutine(reloadTimer(currentGun.reloadTime));
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            SaveAmmo(0);
        }else if (Input.GetKeyDown(KeyCode.Alpha2)){
            SaveAmmo(1);
        }
    }
    private void SaveAmmo(int newGun){
        int lastGun = -1;
        for (int i = 0; i < allGuns.Length; i++)
        {
            if (currentGun == allGuns[i]){
                lastGun = i;
            }
        }
        ammoInEachGun[lastGun] = currentAmmo;
        currentGun = allGuns[newGun];
        currentAmmo = ammoInEachGun[newGun];
    }
    private IEnumerator shootTimer(float fireRate){
        yield return new WaitForSeconds(1/fireRate);
        canShoot = true;
    }
    private IEnumerator reloadTimer(float reloadTime){
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = currentGun.magSize;
        canShoot = true;
        print(currentAmmo);
    }
}
