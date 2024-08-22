using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;

public class shootScript : AttributesSync
{
    [SerializeField] private CustomGun[] allGuns;
    [SerializeField] private ActiveGun[] gunGameObjects;
    [SerializeField] private List<int> ammoInEachGun = new List<int>();
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject bloodSplatterEffect;
    [SerializeField] private GameObject[] muzzleFlash;
    [SerializeField] private GameObject BulletFX;

    private CustomGun currentGun;
    private bool canShoot = true;
    private GameObject holdFlash;
    private int currentAmmo;

    private void Awake(){
        currentGun = allGuns[0];
        currentAmmo = currentGun.magSize;
        for (int i = 0; i < allGuns.Length; i++){
            ammoInEachGun[i] = allGuns[i].magSize;
        }
    }

    private void Update(){
        if (transform.parent.GetComponent<Alteruna.Avatar>().IsMe){
            HandleShooting();
            HandleReloading();
            HandleWeaponSwitch();
        }
    }

    private void HandleShooting(){
        if (GameManager.Instance.gameState == GameManager.GameState.Playing || GameManager.Instance.gameState == GameManager.GameState.ScoreBoard){
            if ((currentGun.gunType == CustomGun.fireType.singleFire && Input.GetMouseButtonDown(0)) ||
                (currentGun.gunType != CustomGun.fireType.singleFire && Input.GetMouseButton(0))){
                
                if (canShoot && currentAmmo > 0){
                    BroadcastRemoteMethod(nameof(PlayShootAnimation));
                    BroadcastRemoteMethod(nameof(ShowMuzzleFlash));
                    PerformRaycast();
                    currentAmmo--;
                    canShoot = false;
                    StartCoroutine(ShootTimer(currentGun.fireRate));
                }
            }
        }
    }

    private void HandleReloading(){
        if (GameManager.Instance.gameState == GameManager.GameState.Playing || GameManager.Instance.gameState == GameManager.GameState.ScoreBoard){
            if (Input.GetKeyDown(KeyCode.R) && currentAmmo < currentGun.magSize){
                canShoot = false;
                BroadcastRemoteMethod(nameof(PlayReloadAnimation));
                StartCoroutine(ReloadTimer(currentGun.reloadTime));
            }
        }
    }

    private void HandleWeaponSwitch(){
        var input = GetNumericKeyInput();
        if (input > 0 && input - 1 < allGuns.Length){
            SaveAmmo(input-1);
            BroadcastRemoteMethod(nameof(VisableWeaponSwitch), input-1);
        }
    }
    [SynchronizableMethod]
    private void VisableWeaponSwitch(int newGun){
        foreach (var gun in gunGameObjects)
        {
            gun.gameObject.SetActive(false);
        }
        gunGameObjects[newGun].gameObject.SetActive(true);
    }
    
    [SynchronizableMethod]
    private void PlayShootAnimation(){
        for (int i = 0; i < transform.GetChild(0).childCount; i++){
            var child = transform.GetChild(0).GetChild(i);
            if (child.gameObject.activeSelf){
                child.GetComponent<Animator>().SetTrigger("Shoot");
            }
        }
    }

    private void PerformRaycast(){
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit)){
            if (hit.transform.CompareTag("Player") && hit.transform.gameObject != parent.gameObject){
                var targetAvatar = hit.transform.gameObject.GetComponent<Alteruna.Avatar>();
                if (GameManager.Instance.GetCurrentTeam(targetAvatar.Owner) != GameManager.Instance.GetCurrentTeam(Multiplayer.GetUser())){
                    hit.transform.gameObject.GetComponent<Health>().TakeHealth(currentGun.damage, parent.gameObject.GetComponent<Alteruna.Avatar>().Possessor);
                    Instantiate(bloodSplatterEffect, hit.point, Quaternion.identity);
                }
            }
        }
    }

    [SynchronizableMethod]
    private void ShowMuzzleFlash(){
        var muzzleSpawn = gunGameObjects[System.Array.IndexOf(allGuns, currentGun)].muzzlePoint;
        holdFlash = Instantiate(muzzleFlash[Random.Range(0, muzzleFlash.Length)], muzzleSpawn.position, muzzleSpawn.transform.rotation * Quaternion.Euler(0, 0, 90));
        holdFlash.transform.parent = muzzleSpawn.transform;
        var bullet = Instantiate(BulletFX, muzzleSpawn.position, Quaternion.identity);
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit)){
            bullet.transform.LookAt(hit.point);
        }else{
            bullet.transform.rotation = muzzleSpawn.rotation;
        }
    }

    [SynchronizableMethod]
    private void PlayReloadAnimation(){
        for (int i = 0; i < transform.GetChild(0).childCount; i++){
            var child = transform.GetChild(0).GetChild(i);
            if (child.gameObject.activeSelf){
                child.GetComponent<Animator>().SetTrigger("Reload");
            }
        }
    }

    private void SaveAmmo(int newGun){
        ammoInEachGun[System.Array.IndexOf(allGuns, currentGun)] = currentAmmo;
        currentGun = allGuns[newGun];
        currentAmmo = ammoInEachGun[newGun];
    }

    private IEnumerator ShootTimer(float fireRate){
        yield return new WaitForSeconds(1 / fireRate);
        canShoot = true;
    }

    private IEnumerator ReloadTimer(float reloadTime){
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = currentGun.magSize;
        canShoot = true;
    }

    private int GetNumericKeyInput()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Playing || GameManager.Instance.gameState == GameManager.GameState.ScoreBoard){
            for (int i = 0; i <= 9; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    print(i);
                    return i;
                }
            }
            return -1;
        } else{
            return -1;
        }
    }
}
