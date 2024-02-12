using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public bool InteractCheck(out string message);

	public void Interact(out ItemBase itemBase);
}
