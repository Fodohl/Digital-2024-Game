using UnityEngine;

[CreateAssetMenu]
public class GameOptions : ScriptableObject
{
    public string prefabName;

    public int numberOfPrefabsToCreate;
    public Vector3[] spawnPoints;
}