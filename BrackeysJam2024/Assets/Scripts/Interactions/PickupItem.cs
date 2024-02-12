using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
	[SerializeField] private ItemBase _item;

	// IInteractable IMPLEMENTATION

	public void Interact(out ItemBase itemBase)
	{
		Destroy(gameObject);

		itemBase = _item;
	}

	public bool InteractCheck()
	{
		//TODO logic

		return true;
	}

}
