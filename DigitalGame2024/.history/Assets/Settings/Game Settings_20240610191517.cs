using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Settings/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    public string prefabName;

    public int numberOfPrefabsToCreate;
    public Vector3[] spawnPoints;
}