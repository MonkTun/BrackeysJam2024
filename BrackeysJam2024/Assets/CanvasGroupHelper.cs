using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupHelper : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public bool isDisplaying;
    [SerializeField] private float transitionRate = 1;
    [SerializeField] private float transitionThreshold = 0.5f;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void Update()
    {
        canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, isDisplaying ? 1 : 0, transitionRate * Time.deltaTime);
        canvasGroup.interactable = canvasGroup.alpha > transitionThreshold;
        canvasGroup.blocksRaycasts = canvasGroup.alpha > transitionThreshold;
    }
    public void ToggleVisibility() { isDisplaying = !isDisplaying; }

    public void TurnOn() { isDisplaying = true; }

    public void TurnOff() { isDisplaying = false; }
}