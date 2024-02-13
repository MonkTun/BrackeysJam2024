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
	[SerializeField] private float _startFadingTime = 0.1f;

	private Transform _lastInteractionPosition;

	private float _lastInteractionTime;

    public void UpdateIteraction(Transform interactionPosition, string itemName)
    {
		_lastInteractionPosition = interactionPosition;

		_interactionText.text = "F - " + itemName;

		_lastInteractionTime = Time.time;

		_interactionCanvasGroup.alpha = 1;

	}

	private void Update()
	{
		if (_lastInteractionPosition != null)
			_interactionUI.transform.position = Camera.main.WorldToScreenPoint(_lastInteractionPosition.position);

		if (_lastInteractionTime + _startFadingTime < Time.time)
		{
			_interactionCanvasGroup.alpha -= Time.deltaTime * _fadeSpeed;
		}
	}
}
