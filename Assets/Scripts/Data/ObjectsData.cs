using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ObjectsData : UpdatableData
{
    public List<ObjectLayer> objectList;
}
[System.Serializable]
public class ObjectLayer
{
    [Range(3, 50)]
    public float radius = 1;
    [Range(1, 50)]
    public int countItems = 30;
    public bool useChunkSizeMultiplier = true;
    [Range(0, 1)]
    public float minHeight = 0;
    [Range(0, 1)]
    public float maxHeight = 1;

    public bool isSpawn = false;
    [Range(3, 10)]
    public int TryToSpawnMultiplier = 5;
    public GameObject objectToSpawn;

    public float YCordOffset = 0f;
    public float ScaleOffset = 0f;
    public List<Vector3> pointsToSpawn;
}