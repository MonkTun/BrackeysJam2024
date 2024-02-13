using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDropActionHandler : MonoBehaviour, IDropHandler  //the handler naming is kinda unfitting
{
	public enum ItemDropActions
	{
		Drop,
		Burn,
		Eat,
		Look
	}

	[SerializeField] private InventoryUI _inventoryUI;

	[SerializeField] private ItemDropActions _dropActions;

	[SerializeField] private TMP_Text _investigateText;

	private PlayerAiming _playerAiming; // lucas you might want to get reference whatever hunger system player has 



	void OnDisable()
	{
		if (_investigateText != null)
			_investigateText.text = string.Empty;
	}

	void IDropHandler.OnDrop(PointerEventData eventData)
	{
		//Debug.Log("OnDrop");

		if (eventData.pointerDrag != null)
		{
			

			var fromSlot = eventData.pointerDrag.GetComponent<UIItemSlot>();

			if (fromSlot != null && fromSlot.InventoryItem != null)
			{
				switch (_dropActions)
				{
					case ItemDropActions.Drop:

						fromSlot.InventoryItem.ItemQuantity--;

						if (_playerAiming == null) _playerAiming = FindObjectOfType<PlayerAiming>();

						if (_playerAiming != null)
						{
							Debug.Log("dropping" + fromSlot.InventoryItem.ItemBase.itemName);
							Instantiate(fromSlot.InventoryItem.ItemBase.pickupItem, _playerAiming.currentPos, Quaternion.identity);
						}

						if (fromSlot.InventoryItem.ItemQuantity <= 0)
						{
							fromSlot.SetItem(null, true);
						}
						else
						{
							fromSlot.SetItem(fromSlot.InventoryItem, true);
						}


						//TODO instantiate item pickup item

						break;

					case ItemDropActions.Burn:

						fromSlot.InventoryItem.ItemQuantity--;

						if (fromSlot.InventoryItem.ItemQuantity <= 0)
						{
							fromSlot.SetItem(null, true);
						}
						else
						{
							fromSlot.SetItem(fromSlot.InventoryItem, true);
						}

						//TODO add to lightmeter

						break;

					case ItemDropActions.Eat:

						fromSlot.InventoryItem.ItemQuantity--;

						if (fromSlot.InventoryItem.ItemQuantity <= 0)
						{
							fromSlot.SetItem(null, true);
						}
						else
						{
							fromSlot.SetItem(fromSlot.InventoryItem, true);
						}

						//TODO add to health

						break;

					case ItemDropActions.Look:

						
						_investigateText.text = fromSlot.InventoryItem.ItemBase.itemName; //TODO this is more of placeholder open dialogue or something

						break;
				}

				
			}

		}
	}
}
