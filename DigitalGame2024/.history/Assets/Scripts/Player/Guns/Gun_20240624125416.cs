using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : ScriptableObject
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
