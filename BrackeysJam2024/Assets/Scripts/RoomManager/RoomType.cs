using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Room",fileName = "RoomData")]
public class RoomType : ScriptableObject
{
    public GameObject roomPrefab;
    [Header("Room Info")]
    public bool hasNorthDoor = false;
    public bool hasSouthDoor = false;
    public bool hasEastDoor = false;
    public bool hasWestDoor = false;
    [Header("Spawn Info")]
    public int weight;
}
