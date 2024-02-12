using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
	[SerializeField] public ItemBase _item;

	// IInteractable IMPLEMENTATION

	public void Interact(out ItemBase itemBase)
	{
		Destroy(gameObject);

		itemBase = _item;
	}

	public bool InteractCheck(out string message)
	{
		//TODO logic
		message = _item.itemName; //other interactive objects can display different message depending on its need

		return true;
	}

}
