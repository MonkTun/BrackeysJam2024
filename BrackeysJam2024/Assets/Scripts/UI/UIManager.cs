using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
	public DialogueUI DialogueUI => _dialogueUI;

	public ViewState State => _state;

	public Animator deathBGAnim;
	public Animator deathTextAnim;
	public GameObject deathButtons;

	[SerializeField] private GameObject gameplayPanel, inventoryPanel, pausedPanel, craftingPanel, settingsPanel, deathPanel, dialoguePanel; 

	private InventoryUI _inventory;
	private PlayerUI _playerUI;
	private InteractionUI _interactionUI;
	private CraftingUI _craftingUI;
	private DialogueUI _dialogueUI;

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
		_dialogueUI = GetComponent<DialogueUI>();
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
				craftingPanel.SetActive(false);
				GameManager.Instance.canPlayerControl = false;

				break;
		}
	}
	public void Death()
    {
		StartCoroutine(ShowDeathUI());
    }
	private IEnumerator ShowDeathUI()
    {
		deathPanel.SetActive(true);
		deathButtons.SetActive(false);
		deathBGAnim.SetTrigger("isDeath");
		deathTextAnim.SetTrigger("isDeath");
		yield return new WaitForSeconds(2);
		Time.timeScale = 0f;
		deathButtons.SetActive(true);
	}

	public void BackToGame()
	{
		ManageGameViews(ViewState.GamePlay);
	}

	public void QuitGame()
	{
		GlobalSceneManager.Instance.OpenGameAsync("MainMenu");
	}

	public void Retry()
    {
		GlobalSceneManager.Instance.OpenGameAsync("Henry3");
	}
}
