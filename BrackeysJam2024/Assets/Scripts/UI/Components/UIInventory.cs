using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
	[SerializeField] private Transform _backpackContent, _hotbarContent;
	[SerializeField] private UIItemSlot _slotPrefab;

	//List<UIItemSlot> uIItemSlots = new List<UIItemSlot>();


	private List<UIItemSlot> _slotsHotbar = new List<UIItemSlot>();
	private List<UIItemSlot> _slotsBackpack = new List<UIItemSlot>();

	private void Start()
	{
		for (int i = 0; i < GlobalVariableHelper.MaxHotbarItems; i++)
		{
			UIItemSlot slot = Instantiate(_slotPrefab, _hotbarContent);
			slot.SetCount(-1);
			slot.SetIcon(null);
			_slotsHotbar.Add(slot);
		}

		for (int i = 0; i < (GlobalVariableHelper.MaxItems - GlobalVariableHelper.MaxHotbarItems); i++)
		{
			UIItemSlot slot = Instantiate(_slotPrefab, _backpackContent);
			slot.SetCount(-1);
			slot.SetIcon(null);
			_slotsBackpack.Add(slot);
		}
	}

	public void InventoryUpdate(List<InventoryItem> hotbarItemList, List<InventoryItem> backpackItemList) //idea: split SetInventory and UpdateInventory
	{
		for (int i = 0; i < _slotsHotbar.Count; i++) // Update Hotbar Slots
		{
			if (i < hotbarItemList.Count)
			{
				print(hotbarItemList[i].ItemQuantity);
				_slotsHotbar[i].SetCount(hotbarItemList[i].ItemQuantity);
				_slotsHotbar[i].SetIcon(hotbarItemList[i].ItemBase.itemIcon);
			}
			else
			{
				_slotsHotbar[i].SetIcon(null);
				_slotsHotbar[i].SetCount(-1);
			}
		}
		
		for (int i = 0; i < _slotsBackpack.Count; i++)
		{
			if ( i < backpackItemList.Count)
			{
				print(backpackItemList[i].ItemQuantity);
				_slotsBackpack[i].SetCount(backpackItemList[i].ItemQuantity);
				_slotsBackpack[i].SetIcon(backpackItemList[i].ItemBase.itemIcon);
			}
			else
			{
				_slotsBackpack[i].SetIcon(null);
				_slotsBackpack[i].SetCount(-1);
			}
		}
	}
}
