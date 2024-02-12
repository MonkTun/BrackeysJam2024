using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class InventoryUI : MonoBehaviour
{
	[SerializeField] private Transform _backpackContent, _hotbarContent;
	[SerializeField] private UIItemSlot _slotPrefab;

	[Header("Drag n Drop")]
	[SerializeField] private Canvas _canvas;
	[SerializeField] private RectTransform _draggerRect;
	[SerializeField] private Image _draggerIconImg;
	[SerializeField] private TMP_Text _draggerCountTxt;

	private List<UIItemSlot> _slotsHotbar = new List<UIItemSlot>();
	private List<UIItemSlot> _slotsBackpack = new List<UIItemSlot>();

	private List<InventoryItem> _hotbarItemList;
	private List<InventoryItem> _backpackItemList;

	private PlayerInventory _playerInventory;

	// MONOBEHAVIOUR

	private void Start()
	{
		for (int i = 0; i < GlobalVariableHelper.MaxHotbarItems; i++)
		{
			UIItemSlot slot = Instantiate(_slotPrefab, _hotbarContent);
			slot.SetItem(null);
			slot.SetUIInventory(this);
			_slotsHotbar.Add(slot);
		}

		for (int i = 0; i < (GlobalVariableHelper.MaxItems - GlobalVariableHelper.MaxHotbarItems); i++)
		{
			UIItemSlot slot = Instantiate(_slotPrefab, _backpackContent);
			slot.SetItem(null);
			slot.SetUIInventory(this);
			_slotsBackpack.Add(slot);
		}
	}

	// PUBLIC METHODS

	public void InventoryUpdate(List<InventoryItem> hotbarItemList, List<InventoryItem> backpackItemList, PlayerInventory playerInventory) //oh now it's so interfucked
	{
		_hotbarItemList = hotbarItemList;
		_backpackItemList = backpackItemList;

		_playerInventory = playerInventory;

		for (int i = 0; i < _slotsHotbar.Count; i++) // Update Hotbar Slots
		{
			if (i < hotbarItemList.Count)
			{
				_slotsHotbar[i].SetItem(hotbarItemList[i]);
			}
			else
			{
				_slotsHotbar[i].SetItem(null);
			}
		}
		
		for (int i = 0; i < _slotsBackpack.Count; i++)
		{
			if ( i < backpackItemList.Count)
			{
				_slotsBackpack[i].SetItem(backpackItemList[i]);
			}
			else
			{
				_slotsBackpack[i].SetItem(null);
			}
		}
	}

	public void DragStart(Sprite sprite, int count, Vector2 startPos)
	{
		_draggerRect.position = new Vector2(startPos.x - 50, startPos.y - 50);
		_draggerRect.gameObject.SetActive(true);
		_draggerIconImg.sprite = sprite;
		_draggerCountTxt.text = count.ToString();
	}

	public void DragUpdate(Vector2 additiveDelta)
	{
		_draggerRect.position += (Vector3)(additiveDelta);
	}

	public void DragEnd()
	{
		_draggerRect.gameObject.SetActive(false);
	}

	public void RefreshInventory()
	{
		_hotbarItemList = new List<InventoryItem>();

		for (int i = 0; i < _slotsHotbar.Count; i++) // this is so scuffed..
		{
			_hotbarItemList.Add(_slotsHotbar[i].InventoryItem);
		}

		_backpackItemList = new List<InventoryItem>();

		for (int i = 0; i < _slotsBackpack.Count; i++)
		{
			_backpackItemList.Add(_slotsBackpack[i].InventoryItem);
		}

		_playerInventory.UpdateInventory(_hotbarItemList, _backpackItemList);
	}
}
