using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject _dialoguePanel;
	[SerializeField] private TMP_Text _dialogueText;
	[SerializeField] private CanvasGroup _canvasGroup;

	private bool _isDialoguePanelOpen;
	private string _currentTargetText;
	private Coroutine _displayCoroutine;

	private bool _isDoneTyping;

	private float _finishedTime;

	public void StartDialogue()
    {
		_dialogueText.text = "";
		_isDialoguePanelOpen = true;
		_dialoguePanel.SetActive(true);
		_isDoneTyping = false;
		_canvasGroup.alpha = 1.0f;	
	}

    public void AddDialogue(string newText) //just add with this
    {
        if (_isDialoguePanelOpen == false) StartDialogue();

		_currentTargetText = newText;
		_canvasGroup.alpha = 1.0f;
		_isDoneTyping = false;

		if (_displayCoroutine != null) StopCoroutine(_displayCoroutine); // Stop the current coroutine if it's running
		_displayCoroutine = StartCoroutine(DisplayText());
	}

	public void EndDialogue()
	{
		_isDialoguePanelOpen = false;
		_dialoguePanel.SetActive(false);
		_dialogueText.text = "";
		_isDoneTyping = false;
	}


	private IEnumerator DisplayText()
	{
		_dialogueText.text = ""; // Reset text
		foreach (char letter in _currentTargetText.ToCharArray())
		{
			_dialogueText.text += letter;
			yield return new WaitForSeconds(0.02f); // Wait for 0.02 seconds before adding the next letter
		}

		_displayCoroutine = null;
		_finishedTime = Time.time;
		_isDoneTyping = true;
	}

	private void Update()
	{
		if (_isDialoguePanelOpen == false) return;
 
		if (Input.GetMouseButton(0))
		{
			if (_displayCoroutine != null)
			{
				StopCoroutine(_displayCoroutine);
				_dialogueText.text = _currentTargetText;
			}
			else
			{
				EndDialogue();
			}

		} 

		if (_isDoneTyping)
		{
			if (_finishedTime + 4 < Time.time)
			{
				EndDialogue();

			} else if (_finishedTime + 2 < Time.time)
			{
				_canvasGroup.alpha -= (Time.deltaTime * 0.5f);
			}

		}
	}


}
