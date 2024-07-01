using UnityEngine;

[CreateAssetMenu(fileName = "GunObject", menuName = "GunObject", order = 51)]
public class CustomGun : ScriptableObject
{
    public float fireRate = 4f;
    public float damage = 10f;
    public float range = 100f;
    public int magSize = 12;
    public enum fireType{
        singleFire,
        autoFire
    }
    public fireType gunType = fireType.singleFire;

}
