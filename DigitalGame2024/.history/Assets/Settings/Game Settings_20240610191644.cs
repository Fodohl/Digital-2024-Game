using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    public string prefabName;

    public int numberOfPrefabsToCreate;
    public Vector3[] spawnPoints;
}