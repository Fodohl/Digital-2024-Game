using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Game Settings", order = 51)]
public class CustomData : ScriptableObject
{
    [Header("Player Settings")]
    [Range(1, 100)]
    public float playerStartHealth = 100;
}
