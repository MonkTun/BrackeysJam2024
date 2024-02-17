using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Item", fileName = "ItemData")]
public class ItemBase : ScriptableObject
{
    [Header("Item Info")]
    public PickupItem pickupItem;
    public string itemName;
    public Sprite itemIcon;
    public int itemQuantity = 1;//how much item quanity per pickup
    public bool isEdible;
    [SerializeField] public int _foodValue;
    public bool isFlammable;
    [SerializeField] private int _fuelValue;
    public int damageMulti;//Amount of damage the object deals
    [SerializeField] private bool isDropped;//An item is dropped when its in the game world, not in the player's inventory or other UI
    public string hint;
}
