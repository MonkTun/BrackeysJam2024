using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
	private PlayerInventory _playerInventory;

	//Should look PlayerInventory and check currently holding item
	// if holding item is weapon type - which doesn't exist yet so you have to add
	// only then you can attack with the weapon
	// it's like minecraft

	private void Awake()
	{
		_playerInventory = GetComponent<PlayerInventory>();
	}

	public void Attack() //just mock up you can change this 
	{

		//_playerInventory.CurrentlySelectedItem <- use this to check currently selected item
	}
}
