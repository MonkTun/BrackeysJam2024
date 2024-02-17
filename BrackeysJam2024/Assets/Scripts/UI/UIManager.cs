using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InventoryUI)), RequireComponent(typeof(PlayerUI)), RequireComponent(typeof(InteractionUI))]
public class UIManager : MonoBehaviour
{
	public enum ViewState
	{
		GamePlay,
		Paused,
		Inventory,
		Crafting,
		Settings,
		Death
	}

    public static UIManager Instance { get; private set; }

	public InventoryUI Inventory => _inventory;
	public PlayerUI PlayerUI => _playerUI;
	public InteractionUI InteractionUI => _interactionUI;
	public CraftingUI CraftingUI => _craftingUI;

	public Slider bandageSlider;


	[SerializeField] private GameObject gameplayPanel, inventoryPanel, pausedPanel, craftingPanel, settingsPanel, deathPanel; 

	private InventoryUI _inventory;
	private PlayerUI _playerUI;
	private InteractionUI _interactionUI;
	private CraftingUI _craftingUI;

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
		_craftingUI = GetComponent<CraftingUI>();
	}

	public void ManageGameViews(ViewState state, bool isToggle = false)
	{
		if (_state == state && state != ViewState.GamePlay && isToggle)
		{
			_state = ViewState.GamePlay;
		} 
		else if (_state == ViewState.Crafting && isToggle)
		{
			_state = ViewState.GamePlay;
		}
		else
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
				craftingPanel.SetActive(false);
				GameManager.Instance.canPlayerControl = true;

				break;
			case ViewState.Inventory:

				gameplayPanel.SetActive(true);
				inventoryPanel.SetActive(true);
				pausedPanel.SetActive(false);
				settingsPanel.SetActive(false);
				deathPanel.SetActive(false);
				craftingPanel.SetActive(false);
				GameManager.Instance.canPlayerControl = false;

				break;

			case ViewState.Crafting:

				gameplayPanel.SetActive(true);
				inventoryPanel.SetActive(true);
				pausedPanel.SetActive(false);
				settingsPanel.SetActive(false);
				deathPanel.SetActive(false);
				craftingPanel.SetActive(true);

				_craftingUI.UpdateCraftability();

				GameManager.Instance.canPlayerControl = false;

				break;

			case ViewState.Paused:

				gameplayPanel.SetActive(false);
				inventoryPanel.SetActive(false);
				pausedPanel.SetActive(true);
				settingsPanel.SetActive(false);
				deathPanel.SetActive(false);
				craftingPanel.SetActive(false);
				GameManager.Instance.canPlayerControl = false;

				break;

			case ViewState.Settings:

				gameplayPanel.SetActive(false);
				inventoryPanel.SetActive(false);
				pausedPanel.SetActive(false);
				settingsPanel.SetActive(true);
				deathPanel.SetActive(false);
				craftingPanel.SetActive(false);
				GameManager.Instance.canPlayerControl = false;

				break;
			case ViewState.Death:

				gameplayPanel.SetActive(false);
				inventoryPanel.SetActive(false);
				pausedPanel.SetActive(false);
				settingsPanel.SetActive(false);
				deathPanel.SetActive(true);
				craftingPanel.SetActive(false);
				GameManager.Instance.canPlayerControl = false;

				break;
		}
	}

	public void UseBandage(float bandageUseDuration)
    {
		StartCoroutine(UBCoroutine(bandageUseDuration));
	}

	private IEnumerator UBCoroutine(float bandageUseDuration)
    {
		//Debug.Log("used bandage");
		bandageSlider.gameObject.SetActive(true);
		bandageSlider.value = 0f;
		float timer = 0f;
		while (timer <= bandageUseDuration)
		{
			timer += Time.deltaTime;
			bandageSlider.value += (Time.deltaTime / bandageUseDuration);
			yield return new WaitForSeconds(Time.deltaTime);
		}
		//Debug.Log("timer: " + timer);
		bandageSlider.gameObject.SetActive(false);
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
