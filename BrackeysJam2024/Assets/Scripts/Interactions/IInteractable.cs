using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public bool InteractCheck();

	public void Interact(out ItemBase itemBase);
}
