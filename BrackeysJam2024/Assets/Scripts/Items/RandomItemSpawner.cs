using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct Item
{
    public GameObject prefab; //Can be none
    public float weight;
}
public class RandomItemSpawner : MonoBehaviour
{
    public Item[] possibleItemList;
    private void Awake()
    {
        float rand = Random.Range(0, CalculateTotalWeight());
        foreach(Item i in possibleItemList)
        {
            rand -= i.weight;
            if (rand < 0)
            {
                Instantiate(i.prefab, transform.position, Quaternion.identity);break;
            }
        }
        Destroy(gameObject);
    }
    public float CalculateTotalWeight()
    {
        float total = 0;foreach(Item i in possibleItemList) { total += i.weight; } return total;
    }
}
