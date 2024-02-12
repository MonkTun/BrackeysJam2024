using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private RectTransform _interactionUI;
    [SerializeField] private TMP_Text _interactionText;
	[SerializeField] private CanvasGroup _interactionCanvasGroup;

	[SerializeField] private float _snapSpeed = 10;
	[SerializeField] private float _fadeSpeed = 2;

	private Vector2 _lastInteractionPosition;

    public void UpdateIteraction(Vector2 interactionPosition, string itemName)
    {
		if (_lastInteractionPosition == interactionPosition)
		{
			_interactionUI.transform.position = Vector2.Lerp(_interactionUI.transform.position, Camera.main.WorldToScreenPoint(interactionPosition), _snapSpeed * Time.deltaTime);
		} 
		else
		{
			_interactionUI.transform.position = Camera.main.WorldToScreenPoint(interactionPosition);
		}

		_lastInteractionPosition = interactionPosition;

		_interactionText.text = "F - " + itemName;

		_interactionCanvasGroup.alpha = 1;
	}

	private void Update()
	{
		_interactionCanvasGroup.alpha -= Time.deltaTime * _fadeSpeed;
	}
}
