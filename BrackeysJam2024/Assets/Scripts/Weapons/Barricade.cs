using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : Weapon
{
	[SerializeField] private PlayerInventory _playerInventory;
	[SerializeField] private ItemBase _barricadeType;

	public override bool Attack(out float staminaCosted, out float hungerCosted)
	{
		if (base.Attack(out float _staminaCosted, out float _hungerCosted))
		{
			staminaCosted = _staminaCosted;
			hungerCosted = _hungerCosted;

			Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 1, 1 << LayerMask.NameToLayer("Wall"));

			foreach (Collider2D col in cols)
			{
				Debug.Log("door found?");

				if (col.TryGetComponent(out DoorHingeHelper door))
				{
					door.LockDoor();
					_playerInventory.RemoveItemFromInventory(_barricadeType, 1);
					return true;
				}
			}

		}

		staminaCosted = 0f;
		hungerCosted = 0f;

		return false;
	}
}
