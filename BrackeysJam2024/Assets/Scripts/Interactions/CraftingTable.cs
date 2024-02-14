using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllIn1SpriteShader;
public class CraftingTable : MonoBehaviour, IInteractable
{



	public void Interact(out ItemBase itemBase)
	{
		itemBase = null;
		UIManager.Instance.ManageGameViews(UIManager.ViewState.Crafting, true);
	}

	public bool InteractCheck(out string message)
	{
		//TODO logic
		message = "Craft"; //other interactive objects can display different message depending on its need

		return true;
	}


}
