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
		Use,
		Look
	}

	[SerializeField] private InventoryUI _inventoryUI;

	[SerializeField] private ItemDropActions _dropActions;

	[SerializeField] private TMP_Text _investigateText;

	private PlayerAiming _playerAiming;

	[SerializeField] private ItemEffects itemEffects;

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
							Debug.Log("dropping " + fromSlot.InventoryItem.ItemBase.itemName);
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
						switch (fromSlot.InventoryItem.ItemBase.itemName)
						{
							case "Paper":
								itemEffects.BurnPaper();
								break;
							case "Alcohol":
								itemEffects.BurnAlcohol();
								break;
							case "Wood Plank":
								itemEffects.BurnWood();
								break;
							case "Bandage":
								itemEffects.BurnBandage();
								break;
							case "Barricade":
								itemEffects.BurnBarricade();
								break;
							default:
								_investigateText.text=fromSlot.InventoryItem.ItemBase.itemName + " cannot be burned!";
								return;
						}
						_investigateText.text = "burned " + fromSlot.InventoryItem.ItemBase.itemName;

						fromSlot.InventoryItem.ItemQuantity--;

						if (fromSlot.InventoryItem.ItemQuantity <= 0)
						{
							fromSlot.SetItem(null, true);
						}
						else
						{
							fromSlot.SetItem(fromSlot.InventoryItem, true);
						}

						break;

					case ItemDropActions.Use:
						switch (fromSlot.InventoryItem.ItemBase.itemName) //Each item has diff text so I set them separately
                        {
							case "Bread":
								itemEffects.EatBread();
								_investigateText.text = "ate bread";
								break;
							case "Raw Meat":
								itemEffects.EatRawMeat();
								_investigateText.text = "ate raw meat...it's poisoned!";
								break;
							case "Cooked Meat":
								itemEffects.EatCookedMeat();
								_investigateText.text = "ate cooked meat...regenerating health!";
								break;
							case "Alcohol":
								itemEffects.DrinkAlcohol();
								_investigateText.text = "drank alcohol...stamina boosted!";
								break;
							case "Bandage":
								itemEffects.UseBandage();
								_investigateText.text = "using bandage...";
								break;
							//TODO Candlestick, Plate, Axe, Mirror, Glass Shard
							default:
								_investigateText.text = fromSlot.InventoryItem.ItemBase.itemName+" cannot be used!";
								return;
						}

						fromSlot.InventoryItem.ItemQuantity--;

						if (fromSlot.InventoryItem.ItemQuantity <= 0)
						{
							fromSlot.SetItem(null, true);
						}
						else
						{
							fromSlot.SetItem(fromSlot.InventoryItem, true);
						}

						break;

					case ItemDropActions.Look:

						
						_investigateText.text = fromSlot.InventoryItem.ItemBase.itemName; //TODO this is more of placeholder open dialogue or something

						break;
				}

				
			}

		}
	}
}