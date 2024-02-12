using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private RectTransform _interactionUI;
    [SerializeField] private TMP_Text _interactionText;
	[SerializeField] private CanvasGroup _interactionCanvasGroup;

    public void UpdateIteraction(Vector2 interactionPosition, string itemName)
    {
		_interactionText.text = "F - " + itemName;

		_interactionCanvasGroup.alpha = 1;


		_interactionUI.transform.position = Camera.main.WorldToScreenPoint(interactionPosition);
	}

	private void Update()
	{
		//_interactionCanvasGroup.alpha _
	}
}
