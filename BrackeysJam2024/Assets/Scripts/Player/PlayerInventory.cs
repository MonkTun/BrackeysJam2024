using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// WRAPPER

public class InventoryItem
{
	public ItemBase ItemBase { get; set; }
	public int ItemQuantity { get; set; }

	public InventoryItem (ItemBase item, int quantity)
	{
		ItemBase = item;
		ItemQuantity = quantity;
	}
 }

public class PlayerInventory : MonoBehaviour
{
	public List<InventoryItem> HotbarItems = new List<InventoryItem>();
	public List<InventoryItem> BackpackItems = new List<InventoryItem>();


	private void Awake()
	{
		SetUpInventory();
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
		InventoryItem existingItem = HotbarItems.FirstOrDefault(x => x?.ItemBase.itemName == item.itemName);
		if (existingItem == null) existingItem = BackpackItems.FirstOrDefault(x => x?.ItemBase.itemName == item.itemName);

		if (existingItem != null)
		{
			
			existingItem.ItemQuantity += item.itemQuantity;

		} 
		else
		{
			// Find the first null (empty) index in the hotbar
			int emptyHotbarIndex = HotbarItems.IndexOf(null);
			if (emptyHotbarIndex != -1)
			{
				// Found an empty slot in the hotbar, add the item there
				HotbarItems[emptyHotbarIndex] = new InventoryItem(item, item.itemQuantity);
			}
			else
			{
				// No empty slot in the hotbar, check the backpack
				int emptyBackpackIndex = BackpackItems.IndexOf(null);
				if (emptyBackpackIndex != -1)
				{
					// Found an empty slot in the backpack, add the item there
					BackpackItems[emptyBackpackIndex] = new InventoryItem(item, item.itemQuantity);
				}
				else
				{
					// No empty slots available in either the hotbar or backpack
					Debug.Log("Inventory Full");
				}
			}
		}

		UIManager.Instance.Inventory.InventoryUpdate(HotbarItems, BackpackItems, this);
	}

	public void RemoveItemFromInventory(ItemBase item, int quantity)
	{
		// Try to remove from the hotbar first
		for (int i = 0; i < HotbarItems.Count; i++)
		{
			if (HotbarItems[i]?.ItemBase.itemName == item.itemName)
			{
				if (HotbarItems[i].ItemQuantity > quantity)
				{
					HotbarItems[i].ItemQuantity -= quantity;
					quantity = 0; // All required quantities have been removed
					break; // Exit the loop as no more removal is needed
				}
				else
				{
					quantity -= HotbarItems[i].ItemQuantity; // Deduct the existing quantity from the total to be removed
					HotbarItems[i] = null; // Remove the item by setting its slot to null
					if (quantity <= 0) break; // If no more quantity needs to be removed, exit the loop
				}
			}
		}

		// If there's still quantity left to remove, try to remove from the backpack
		if (quantity > 0)
		{
			for (int i = 0; i < BackpackItems.Count; i++)
			{
				if (BackpackItems[i]?.ItemBase.itemName == item.itemName)
				{
					if (BackpackItems[i].ItemQuantity > quantity)
					{
						BackpackItems[i].ItemQuantity -= quantity;
						quantity = 0; // All required quantities have been removed
						break; // Exit the loop as no more removal is needed
					}
					else
					{
						quantity -= BackpackItems[i].ItemQuantity; // Deduct the remaining quantity
						BackpackItems[i] = null; // Remove the item by setting its slot to null
						if (quantity <= 0) break; // If no more quantity needs to be removed, exit the loop
					}
				}
			}
		}
		//UIManager.Instance.Inventory.InventoryUpdate(HotbarItems, BackpackItems, this);
	}

	public void SetUpInventory() //TODO load from saves; right now I am just filling it with null
	{
		HotbarItems = new List<InventoryItem>();
		BackpackItems = new List<InventoryItem>();

		for (int i = 0; i < GlobalVariableHelper.MaxHotbarItems; i++)
		{
			HotbarItems.Add(null);
		}

		for (int i = 0; i < (GlobalVariableHelper.MaxItems - GlobalVariableHelper.MaxHotbarItems); i++)
		{
			BackpackItems.Add(null);
		}
	}

	public void UpdateInventory(List<InventoryItem> hotbarItem, List<InventoryItem> backpackItem)
	{
		HotbarItems = hotbarItem;
		BackpackItems = backpackItem;
	}

}
