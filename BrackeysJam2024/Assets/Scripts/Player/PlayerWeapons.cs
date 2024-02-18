using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInventory), typeof(HungerManager), typeof(StaminaManager))]
public class PlayerWeapons : MonoBehaviour
{
	[SerializeField] private Axe axe;
	[SerializeField] private Barricade barricade;

	private PlayerInventory _playerInventory;
	private HungerManager _hungerManager;
	private StaminaManager _staminaManager;

	private void Awake()
	{
		_playerInventory = GetComponent<PlayerInventory>();
		_staminaManager = GetComponent<StaminaManager>();
		_hungerManager = GetComponent<HungerManager>();
	}

	private void Update()
	{
		UpdateWeapon();

		if (Input.GetMouseButtonDown(0) && GameManager.Instance.canPlayerControl)
		{
			TryAttack();
		}
	}

	
	private void UpdateWeapon()
	{
		if (_playerInventory.CurrentlySelectedItem != null  && _playerInventory.CurrentlySelectedItem.ItemBase != null)
		{
			axe.gameObject.SetActive(_playerInventory.CurrentlySelectedItem.ItemBase.itemName == "Axe");
			barricade.gameObject.SetActive(_playerInventory.CurrentlySelectedItem.ItemBase.itemName == "Barricade");
		}
	}


	private void TryAttack()
	{
		if (_staminaManager.stamina <= 10 || _hungerManager.hunger <= 0) return;

		if (axe.gameObject.activeInHierarchy)
		{
			if (axe.Attack(out float staminaCosted, out float hungerCosted))
			{
				_staminaManager.changeStamina(-staminaCosted);
				_hungerManager.changeHunger(-hungerCosted);
			}
		}
		else if (barricade.gameObject.activeInHierarchy)
		{
			if (barricade.Attack(out float staminaCosted, out float hungerCosted))
			{
				_staminaManager.changeStamina(-staminaCosted);
				_hungerManager.changeHunger(-hungerCosted);
			}
		}

		//_playerInventory.CurrentlySelectedItem <- use this to check currently selected item
	}
}
