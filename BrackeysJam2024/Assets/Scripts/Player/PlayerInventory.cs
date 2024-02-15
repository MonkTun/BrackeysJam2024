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

[RequireComponent(typeof(PlayerInventory))]
public class PlayerInventory : MonoBehaviour
{
	public List<InventoryItem> HotbarItems = new List<InventoryItem>();
	public List<InventoryItem> BackpackItems = new List<InventoryItem>();

	public InventoryItem CurrentlySelectedItem => HotbarItems[_currentSlot]; 

	private int _currentSlot;

	private PlayerAiming _playerAiming;

	private void Awake()
	{
		_playerAiming = GetComponent<PlayerAiming>();

		SetUpInventory();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			UIManager.Instance.ManageGameViews(UIManager.ViewState.Inventory, true);
		}

		if (GameManager.Instance.canPlayerControl == false) return;

		for (int i = (int)KeyCode.Alpha0; i <= (int)KeyCode.Alpha5; i++)
		{
			if (Input.GetKey((KeyCode)i) == true)
			{
				_currentSlot = (byte)(i - (int)KeyCode.Alpha0 - 1);
				UIManager.Instance.Inventory.SelectHotbarSlot(_currentSlot);
				return;
			}
		}

		int mouseWheelSlot = GetMouseWheel();

		if (mouseWheelSlot != -1)
		{
			_currentSlot = mouseWheelSlot;
			UIManager.Instance.Inventory.SelectHotbarSlot(_currentSlot);
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
					Debug.Log("Inventory Full, dropping " + item.itemName);
					Instantiate(item.pickupItem, _playerAiming.currentPos, Quaternion.identity);
					
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
		UIManager.Instance.Inventory.InventoryUpdate(HotbarItems, BackpackItems, this);
	}

	public bool CheckItemFromInventory(ItemBase item, int quantity)
	{
		int foundQuantity = 0;

		for (int i = 0; i < HotbarItems.Count; i++)
		{
			if (HotbarItems[i]?.ItemBase.itemName == item.itemName)
			{
				foundQuantity += HotbarItems[i].ItemQuantity;
			}
		}

		for (int i = 0; i < BackpackItems.Count; i++)
		{
			if (BackpackItems[i]?.ItemBase.itemName == item.itemName)
			{
				foundQuantity += BackpackItems[i].ItemQuantity;
			}
		}

		return (foundQuantity >= quantity);
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

	// PRIVATE METHODS

	private int GetMouseWheel()
	{
		float wheelAxis = Input.GetAxis("Mouse ScrollWheel");

		if (wheelAxis == 0f)
			return -1;

		int weaponButton = 0;

		if (wheelAxis < 0f) //TODO option for the inverse
		{
			weaponButton = _currentSlot + 1;

			if (weaponButton >= GlobalVariableHelper.MaxHotbarItems)
			{
				weaponButton = 0;
			}
		}
		else if (wheelAxis > 0f)
		{
			weaponButton = _currentSlot - 1;

			if (weaponButton < 0)
			{
				weaponButton = GlobalVariableHelper.MaxHotbarItems - 1;
			}
		}

		

		return weaponButton;
	}
}
