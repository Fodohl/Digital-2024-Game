using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Game Settings", order = 51)]
public class CustomData : ScriptableObject
{
    public int someInt;
    public float someFloat;
    public string someString;
}
