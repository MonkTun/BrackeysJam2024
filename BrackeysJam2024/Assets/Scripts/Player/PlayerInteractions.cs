using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles interactions
/// </summary>
[RequireComponent(typeof(PlayerInventory))]
public class PlayerInteractions : MonoBehaviour
{
	// PRIVATE MEMBERS

    [SerializeField] private Transform _root;
    [SerializeField] private float _interactDistance = 3;
    [SerializeField] private LayerMask _interactMask;

	private PlayerInventory _inventory;

    private IInteractable _selectedInteractable;

	// MONOBEHAVIOURS

	private void Awake()
	{
		_inventory = GetComponent<PlayerInventory>();
	}

	private void FixedUpdate()
    {
        FindNearbyInteractions();

	}

	private void Update()
	{
		TryInteract();
	}

	// PRIVATE MEHOTDS

	private void TryInteract()
	{
		if (_selectedInteractable == null) return;

		if (Input.GetKeyDown(KeyCode.F))
		{
			_selectedInteractable.Interact(out ItemBase newItem);

			if (newItem != null)
			{
				Debug.Log(newItem.itemName);
				_inventory.AddItemToInventory(newItem);
			}

			_selectedInteractable = null;
		}
	}

	private void FindNearbyInteractions()
    {
		RaycastHit2D[] hits = Physics2D.RaycastAll(_root.position, _root.right, _interactDistance, _interactMask);

		foreach (RaycastHit2D hit in hits)
		{
			if (hit.transform.TryGetComponent(out IInteractable interactable))
			{
				if (interactable.InteractCheck(out string message)) 
				{
					_selectedInteractable = interactable;
					UIManager.Instance.InteractionUI.UpdateIteraction(hit.transform, message);
				}

				return;
			}
		}
	}

	/*
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(_root.position, _root.right * _interactDistance);
	}*/
}
