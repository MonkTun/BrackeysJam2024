using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

/// <summary>
/// DEPRECATED
/// </summary>
public class UIItemSlot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
	[SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Image _iconImg;
	[SerializeField] private TMP_Text _countTxt;
	[SerializeField] private Image _highlighter;

	public InventoryItem InventoryItem { get; private set; } //can this be private?

	private InventoryUI _uiInventory;

	// PUBLIC METHODS

	public void SetUIInventory(InventoryUI uiInventory)
	{
		_uiInventory = uiInventory;
	}

	public void SetItem(InventoryItem newItem)
	{
		InventoryItem = newItem;

		if (newItem != null)
		{
			SetIcon(newItem.ItemBase.itemIcon);
			SetCount(newItem.ItemQuantity);
		} else
		{
			SetIcon(null);
			SetCount(-1);
		}
	}
	public void SetHighlighter(bool enable)
	{
		_highlighter.gameObject.SetActive(enable);
	}

	// PRIVATE METHODS

	private void SetIcon(Sprite icon)
	{
		_iconImg.gameObject.SetActive(icon != null);
		_iconImg.sprite = icon;
	}

	private void SetCount(int count)
	{
			
		_countTxt.text = count <= 1 ? string.Empty : count.ToString();
	}


	// EVENTSYSTEM IMPLEMENTATIONS

	public void OnBeginDrag(PointerEventData eventData)
	{
		//Debug.Log("OnBeginDrag");

		if (InventoryItem == null) return;

		_uiInventory.DragStart(InventoryItem.ItemBase.itemIcon, InventoryItem.ItemQuantity, transform.position);

		//_canvasGroup.blocksRaycasts = false;
		//_canvasGroup.alpha = 0.6f;
	}

	void IDragHandler.OnDrag(PointerEventData eventData)
	{
		//Debug.Log("OnDrag");

		if (InventoryItem == null) return;

		_uiInventory.DragUpdate(eventData.delta);
		//_dragRectTransform.anchoredPosition += eventData.delta/* / _canvas.scaleFactor*/;
	}

	void IEndDragHandler.OnEndDrag(PointerEventData eventData)
	{
		//Debug.Log("OnEndDrag");

		_uiInventory.DragEnd();
		//_canvasGroup.alpha = 1;
		//_canvasGroup.blocksRaycasts = true;
	}

	void IDropHandler.OnDrop(PointerEventData eventData)
	{
		//Debug.Log("OnDrop");

		if (eventData.pointerDrag != null) 
		{
			var toSwapSlot = eventData.pointerDrag.GetComponent<UIItemSlot>();

			if (toSwapSlot != null)
			{
				var newItem = toSwapSlot.InventoryItem;

				toSwapSlot.SetItem(InventoryItem);

				if (newItem != null)
				{
					SetItem(newItem);
				}

			}

		}
		_uiInventory.RefreshInventory();
	}
}
