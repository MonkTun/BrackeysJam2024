using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    [Header("Item Info")]
    public string itemName;
    public bool isEdible;
    [SerializeField] private int _foodValue;
    public bool isFlammable;
    [SerializeField] private int _fuelValue;
    public int damageMulti;//Amount of damage the object deals
    [SerializeField] private bool isDropped;//An item is dropped when its in the game world, not in the player's inventory or other UI


}
