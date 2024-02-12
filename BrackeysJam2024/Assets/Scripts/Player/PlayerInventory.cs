using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// WRAPPER

public class InventoryItem
{
	public ItemBase ItemBase { get; set; }
	public int ItemQuantity { get; set; }
	public bool IsInHotbar { get; set; }

	public InventoryItem (ItemBase item, int quantity, bool isInHotbar)
	{
		ItemBase = item;
		ItemQuantity = quantity;
		IsInHotbar = isInHotbar;
	}
 }

public class PlayerInventory : MonoBehaviour
{
	public List<InventoryItem> AllItems => _allItems;
	public List<InventoryItem> HotbarItems => _allItems.Where(i => i.IsInHotbar).ToList(); 
	public List<InventoryItem> BackpackItems => _allItems.Where(i => !i.IsInHotbar).ToList();

	private List<InventoryItem> _allItems = new List<InventoryItem>();

	

	private void Awake()
	{
		//TODO saving and loading inventory here
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			UIManager.Instance.ManageGameViews(UIManager.ViewState.Inventory, true);
		}
	}


	// PUBLIC METHODS

	public void AddItemToInventory(ItemBase item)
	{
		InventoryItem existingItem = _allItems.FirstOrDefault(x => x.ItemBase.itemName == item.itemName);
		if (existingItem != null)
		{

			existingItem.ItemQuantity += item.itemQuantity;
			UIManager.Instance.Inventory.InventoryUpdate(HotbarItems, BackpackItems);

			return;
		}

		if (_allItems.Count < GlobalVariableHelper.MaxItems)
		{
			_allItems.Add(new InventoryItem(item, item.itemQuantity, HotbarItems.Count() < GlobalVariableHelper.MaxHotbarItems));

			UIManager.Instance.Inventory.InventoryUpdate(HotbarItems, BackpackItems);
		} 
		else
		{
			Debug.Log("Inventory Full"); //TODO show UI saying this
		}
	}

	public void RemoveItemFromInventory(ItemBase item, int quantity)
	{
		InventoryItem existingItem = _allItems.FirstOrDefault(x => x.ItemBase.itemName == item.itemName);
		if (existingItem != null)
		{
			existingItem.ItemQuantity -= quantity;

			if (existingItem.ItemQuantity <= 0)
			{
				_allItems.Remove(existingItem);
			}
		}
	}
}
