using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "GunObject", menuName = "GunObject", order = 51)]
public class CustomGun : ScriptableObject
{
    public float fireRate = 4f;
    public float damage = 10f;
    public int magSize = 12;
    public float reloadTime = 1.5f;
    public float putAwayTime = 0.5f;
    public float pullOutTime = 0.5f;
    public enum fireType{
        singleFire,
        autoFire
    }
    public fireType gunType = fireType.singleFire;

}
