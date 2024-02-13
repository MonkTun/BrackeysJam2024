using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// DEPRECATED
/// </summary>
public class UIItemDragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
	private RectTransform _rectTransform;
	private CanvasGroup _canvasGroup;

	// PRIVATE METHODS

	private void Awake()
	{
		_rectTransform = GetComponent<RectTransform>();
		_canvasGroup = GetComponent<CanvasGroup>();
	}

	// EVENTSYSTEM IMPLEMENTATIONS

	public void OnBeginDrag(PointerEventData eventData)
	{
		_canvasGroup.blocksRaycasts = false;
		_canvasGroup.alpha = 0.6f;
	}

	void IDragHandler.OnDrag(PointerEventData eventData)
	{
		_rectTransform.anchoredPosition += eventData.delta/* / _canvas.scaleFactor*/;
	}

	void IEndDragHandler.OnEndDrag(PointerEventData eventData)
	{
		_canvasGroup.alpha = 1;
		_canvasGroup.blocksRaycasts = true;
	}


	void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
	{
		//throw new System.NotImplementedException();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		//throw new System.NotImplementedException();
	}
}
