using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIInventory)), RequireComponent(typeof(PlayerUI))]
public class UIManager : MonoBehaviour
{
	public enum ViewState
	{
		GamePlay,
		Paused,
		Inventory,
		Settings,
		Death
	}

    public static UIManager Instance { get; private set; }

	public UIInventory Inventory => _inventory;
	public PlayerUI PlayerUI => _playerUI;

	[SerializeField] private GameObject gameplayPanel, inventoryPanel, pausedPanel, settingsPanel, deathPanel; 

	private UIInventory _inventory;
	private PlayerUI _playerUI;
	private ViewState _state;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else 
		{ 
			Destroy(gameObject);
			return;
		}

		_inventory = GetComponent<UIInventory>();
		_playerUI = GetComponent<PlayerUI>();
	}

	public void ManageGameViews(ViewState state, bool isToggle = false)
	{
		if (_state == state && state != ViewState.GamePlay && isToggle)
		{
			_state = ViewState.GamePlay;
		} else
		{
			_state = state;
		}

		switch (_state)
		{
			case ViewState.GamePlay: //Maybe pasued and settings should be in systemmanager
				
				gameplayPanel.SetActive(true);
				inventoryPanel.SetActive(false);
				pausedPanel.SetActive(false);
				settingsPanel.SetActive(false);
				deathPanel.SetActive(false);

				break;
			case ViewState.Inventory:

				gameplayPanel.SetActive(true);
				inventoryPanel.SetActive(true);
				pausedPanel.SetActive(false);
				settingsPanel.SetActive(false);
				deathPanel.SetActive(false);

				break;

			case ViewState.Paused:

				gameplayPanel.SetActive(false);
				inventoryPanel.SetActive(false);
				pausedPanel.SetActive(true);
				settingsPanel.SetActive(false);
				deathPanel.SetActive(false);

				break;

			case ViewState.Settings:

				gameplayPanel.SetActive(false);
				inventoryPanel.SetActive(false);
				pausedPanel.SetActive(false);
				settingsPanel.SetActive(true);
				deathPanel.SetActive(false);

				break;
			case ViewState.Death:

				gameplayPanel.SetActive(false);
				inventoryPanel.SetActive(false);
				pausedPanel.SetActive(false);
				settingsPanel.SetActive(false);
				deathPanel.SetActive(true);

				break;
		}
	}

	//TODO later if you guys want we should split these //UPDATE: I am splitting these
/*
	public void InventoryAddItem()
	{

	}
	public void InventoryRemoveItem()
	{

	}

	public void UpdateHotbar()
	{

	}

	public void UpdateHealth(float health, float maxHealth)
	{

	}*/
}
