using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InventoryUI)), RequireComponent(typeof(PlayerUI)), RequireComponent(typeof(InteractionUI))]
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

	public InventoryUI Inventory => _inventory;
	public PlayerUI PlayerUI => _playerUI;
	public InteractionUI InteractionUI => _interactionUI;


	[SerializeField] private GameObject gameplayPanel, inventoryPanel, pausedPanel, settingsPanel, deathPanel; 

	private InventoryUI _inventory;
	private PlayerUI _playerUI;
	private InteractionUI _interactionUI;

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

		_inventory = GetComponent<InventoryUI>();
		_playerUI = GetComponent<PlayerUI>();
		_interactionUI = GetComponent<InteractionUI>();
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
				GameManager.Instance.canPlayerControl = true;

				break;
			case ViewState.Inventory:

				gameplayPanel.SetActive(true);
				inventoryPanel.SetActive(true);
				pausedPanel.SetActive(false);
				settingsPanel.SetActive(false);
				deathPanel.SetActive(false);
				GameManager.Instance.canPlayerControl = false;

				break;

			case ViewState.Paused:

				gameplayPanel.SetActive(false);
				inventoryPanel.SetActive(false);
				pausedPanel.SetActive(true);
				settingsPanel.SetActive(false);
				deathPanel.SetActive(false);
				GameManager.Instance.canPlayerControl = false;

				break;

			case ViewState.Settings:

				gameplayPanel.SetActive(false);
				inventoryPanel.SetActive(false);
				pausedPanel.SetActive(false);
				settingsPanel.SetActive(true);
				deathPanel.SetActive(false);
				GameManager.Instance.canPlayerControl = false;

				break;
			case ViewState.Death:

				gameplayPanel.SetActive(false);
				inventoryPanel.SetActive(false);
				pausedPanel.SetActive(false);
				settingsPanel.SetActive(false);
				deathPanel.SetActive(true);
				GameManager.Instance.canPlayerControl = false;

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
