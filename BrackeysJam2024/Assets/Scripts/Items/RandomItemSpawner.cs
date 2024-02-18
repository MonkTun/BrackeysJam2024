using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[System.Serializable]
public struct Item
{
    public GameObject prefab; //Can be none
    public float weight;
}
public class RandomItemSpawner : MonoBehaviour
{
    [SerializeField] private int startingItemAmount; //amount spawned at game start
    [SerializeField] private float minSpawnInterval;
    [SerializeField] private float maxSpawnInterval;
    [SerializeField] private int minSpawnAmount;
    [SerializeField] private int maxSpawnAmount;
    [SerializeField] private float minItemSeparation; //minimum distance between items
    public Item[] possibleItemList;

    void Start()
    {
        //Debug.Log("Point: " + GetRandomPointOnNavMesh());
        SpawnXItems(startingItemAmount);
        StartCoroutine(ProceduralSpawnItems());
    }

    private IEnumerator ProceduralSpawnItems()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
            SpawnXItems((int)Random.Range(minSpawnAmount, maxSpawnAmount));
        }
    }

    private void SpawnXItems(int x)
    {
        Vector3 position;
        Vector3[] previousPositions=new Vector3[x];
        for (int i = 0; i < x; i++)
        {
            do
            {
                position = GetRandomPointOnNavMesh();
            }
            while (!CheckSpawnDistance(previousPositions, position));
            SpawnRandomItem(new Vector3(position.x, position.y, 0f));
            previousPositions[i] = position;
        }
    }

    private bool CheckSpawnDistance(Vector3[] previousPositions, Vector3 position)
    {
        foreach (Vector3 previousPosition in previousPositions)
        {
            if (previousPosition != null && Vector3.Distance(previousPosition,position)<=minItemSeparation)
            {
                return false;
            }
        }
        return true;
    }

    private void SpawnRandomItem(Vector3 position)
    {
        float rand = Random.Range(0, CalculateTotalWeight());
        foreach(Item i in possibleItemList)
        {
            rand -= i.weight;
            if (rand < 0)
            {
                Instantiate(i.prefab, position, Quaternion.identity);break;
            }
        }
        Destroy(gameObject);
    }
    private float CalculateTotalWeight()
    {
        float total = 0;foreach(Item i in possibleItemList) { total += i.weight; } return total;
    }

    private Vector3 GetRandomPointOnNavMesh()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        // Pick a random triangle from the NavMesh's triangles
        int randomTriangleIndex = Random.Range(0, navMeshData.indices.Length / 3);
        int index = randomTriangleIndex * 3;

        // Get the vertices of the triangle
        Vector3 vertex1 = navMeshData.vertices[navMeshData.indices[index]];
        Vector3 vertex2 = navMeshData.vertices[navMeshData.indices[index + 1]];
        Vector3 vertex3 = navMeshData.vertices[navMeshData.indices[index + 2]];

        // Generate random barycentric coordinates within the triangle
        float r1 = Random.Range(0f, 1f);
        float r2 = Random.Range(0f, 1f - r1);

        // Calculate the point using the barycentric coordinates
        Vector3 randomPoint = vertex1 + r1 * (vertex2 - vertex1) + r2 * (vertex3 - vertex1);

        return randomPoint;
    }
}
