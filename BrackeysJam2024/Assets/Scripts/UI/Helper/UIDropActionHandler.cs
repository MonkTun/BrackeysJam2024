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

	//[SerializeField] private TMP_Text _investigateText;

	private PlayerAiming _playerAiming;
	private ItemEffects _itemEffects;

	void OnDisable()
	{
		//if (_investigateText != null)
		//_investigateText.text = string.Empty;

		//UIManager.Instance.DialogueUI.EndDialogue();
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

						break;

					case ItemDropActions.Burn:

						if (_itemEffects == null) _itemEffects = FindObjectOfType<ItemEffects>();

						switch (fromSlot.InventoryItem.ItemBase.itemName)
						{
							case "Paper":
								_itemEffects.BurnPaper();
								break;
							case "Alcohol":
								_itemEffects.BurnAlcohol();
								break;
							case "Wood Plank":
								_itemEffects.BurnWood();
								break;
							case "Bandage":
								_itemEffects.BurnBandage();
								break;
							case "Barricade":
								_itemEffects.BurnBarricade();
								break;
							default:
								UIManager.Instance.DialogueUI.AddDialogue(fromSlot.InventoryItem.ItemBase.itemName + " cannot be burned!");
								return;
						}
						UIManager.Instance.DialogueUI.AddDialogue("burned " + fromSlot.InventoryItem.ItemBase.itemName);

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

						if (_itemEffects == null) _itemEffects = FindObjectOfType<ItemEffects>();

						switch (fromSlot.InventoryItem.ItemBase.itemName) //Each item has diff text so I set them separately
                        {
							case "Bread":
								_itemEffects.EatBread();
								UIManager.Instance.DialogueUI.AddDialogue("ate bread");
								break;
							case "Raw Meat":
								_itemEffects.EatRawMeat();
								UIManager.Instance.DialogueUI.AddDialogue("ate raw meat...it's poisoned!");
								break;
							case "Cooked Meat":
								_itemEffects.EatCookedMeat();
								UIManager.Instance.DialogueUI.AddDialogue("ate cooked meat...regenerating health!");
								break;
							case "Alcohol":
								_itemEffects.DrinkAlcohol();
								UIManager.Instance.DialogueUI.AddDialogue("drank alcohol...stamina boosted!");
								break;
							case "Bandage":
								_itemEffects.UseBandage();
								UIManager.Instance.DialogueUI.AddDialogue("using bandage...");
								break;
							//TODO Candlestick, Plate, Axe, Mirror, Glass Shard
							default:
								UIManager.Instance.DialogueUI.AddDialogue(fromSlot.InventoryItem.ItemBase.itemName + " cannot be used!");
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
						if (fromSlot.InventoryItem.ItemBase.hint != "")
                        {
							UIManager.Instance.DialogueUI.AddDialogue(fromSlot.InventoryItem.ItemBase.hint);
						}
                        else
                        {
							UIManager.Instance.DialogueUI.AddDialogue(fromSlot.InventoryItem.ItemBase.itemName);
						}

						break;
				}

				
			}

		}
	}
}
