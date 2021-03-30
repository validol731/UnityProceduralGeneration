using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ObjectsGenerator : MonoBehaviour
{
    [System.Obsolete]
    public static void GeneratePointsRandom(List<ObjectLayer> objectLayer, Mesh mesh, MeshSettings meshSettings, int seed)
    {
        Random.seed = seed;

        for (int k = 0; k < objectLayer.Count; k++)
        {
            objectLayer[k].pointsToSpawn = new List<Vector3>();
            int itemscount = objectLayer[k].countItems;
            if (objectLayer[k].useChunkSizeMultiplier)
                itemscount = objectLayer[k].countItems * (meshSettings.chunkSizeIndex + 1);

            int tryToCreate = itemscount * objectLayer[k].TryToSpawnMultiplier;

            while (itemscount > 0 && tryToCreate > 0)
            {
                int rand = Random.Range(0, mesh.vertices.Length);
                Vector3 vertex = mesh.vertices[rand];
                float vertexY = Mathf.InverseLerp(mesh.bounds.min.y, mesh.bounds.max.y, vertex.y);
                if (vertexY > objectLayer[k].minHeight && vertexY < objectLayer[k].maxHeight)
                {
                    bool isCandidate = true;
                    for (int j = 0; j < objectLayer.Count; j++)
                    {
                        for (int i = 0; i < objectLayer[j].pointsToSpawn.Count; i++)
                        {
                            float sqrDst = (new Vector2(vertex.x, vertex.z) - new Vector2(objectLayer[j].pointsToSpawn[i].x, objectLayer[j].pointsToSpawn[i].z)).sqrMagnitude;
                            if (sqrDst < objectLayer[j].radius * 5)
                            {
                                isCandidate = false;
                                break;
                            }
                        }
                    }
                    if (isCandidate)
                    {
                        objectLayer[k].pointsToSpawn.Add(new Vector3(vertex.x, vertex.y + objectLayer[k].YCordOffset, vertex.z));
                        itemscount--;
                    }
                }
                tryToCreate--;
            }
        }
    }

    [System.Obsolete]
    public static void SpawnObjects(ObjectsData objectData, Transform parent, MeshSettings meshSettings, int seed)
    {
        Random.seed = seed;

        while (true)
        {
            if (parent.FindChild("ObjectsWorld")) 
            {
                DestroyImmediate(parent.FindChild("ObjectsWorld").gameObject);
            }
            else { break; }
        }
        GameObject objPreParent = new GameObject("ObjectsWorld");
        objPreParent.transform.parent = parent;
        objPreParent.transform.position = parent.position;

        for (int i = 0; i < objectData.objectList.Count; i++)
        {
            if (objectData.objectList[i].pointsToSpawn.Count > 0 && objectData.objectList[i].isSpawn)
            {

                GameObject objParent = new GameObject();
                if (objectData.objectList[i].objectToSpawn == null)
                {
                    objParent.name = "Default Cube";
                }
                else
                {
                    objParent.name = objectData.objectList[i].objectToSpawn.name;
                }
                objParent.transform.parent = objPreParent.transform;
                objParent.transform.position = objPreParent.transform.position;
                objParent.transform.localPosition = Vector3.zero;
                for (int k = 0; k < objectData.objectList[i].pointsToSpawn.Count; k++)
                {
                    Vector3 position = new Vector3(objectData.objectList[i].pointsToSpawn[k].x, objectData.objectList[i].pointsToSpawn[k].y, objectData.objectList[i].pointsToSpawn[k].z);
                    GameObject objChild;
                    if (objectData.objectList[i].objectToSpawn == null)
                    {
                        objChild = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        objChild.name = "Default Cube Child " + k;
                    }
                    else
                    {
                        objChild = Instantiate(objectData.objectList[i].objectToSpawn);
                        objChild.name = objectData.objectList[i].objectToSpawn.name + " - " + k + 1;
                    }
                    objChild.transform.parent = objParent.transform;
                    objChild.transform.localPosition = position;
                    objChild.transform.rotation = new Quaternion(objChild.transform.rotation.x, objChild.transform.rotation.y + Random.Range(0, 10), objChild.transform.rotation.z, objChild.transform.rotation.w);
                    objChild.transform.localScale = new Vector3(objChild.transform.localScale.x + objectData.objectList[i].ScaleOffset + Random.Range(0, 1),
                                                                objChild.transform.localScale.y + objectData.objectList[i].ScaleOffset + Random.Range(0, 1),
                                                                objChild.transform.localScale.z + objectData.objectList[i].ScaleOffset + Random.Range(0, 1));
                }
            }
        }
    }

}