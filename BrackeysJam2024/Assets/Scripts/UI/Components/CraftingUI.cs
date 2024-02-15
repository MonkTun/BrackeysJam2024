using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    [SerializeField] private CraftingRecipe[] _craftingRecipes;

    [SerializeField] private Transform _craftingTableContent;
    [SerializeField] private UICraftingOptionSlot _craftingOptionPrefab;

    [SerializeField] private RectTransform _informationWindow;

	//[SerializeField] private Button _craftButton;

	private UICraftingOptionSlot _selectedCraftingOptonSlot;

	private List<UICraftingOptionSlot> _instantiatedSlots = new List<UICraftingOptionSlot>();

	private PlayerInventory _playerInventory;

    // MONOBEHAVIOURS

	private void Start()
    {
		foreach (CraftingRecipe recipe in _craftingRecipes)
		{
			UICraftingOptionSlot optionSlot = Instantiate(_craftingOptionPrefab, _craftingTableContent);

			optionSlot.SetUp(recipe, this);

			_instantiatedSlots.Add(optionSlot);
		}

		SelectItem(null);
	}


	// PUBLIC METHODS

	public void SelectItem(UICraftingOptionSlot toSelectslot)
	{
		_selectedCraftingOptonSlot = toSelectslot;

		foreach (UICraftingOptionSlot slot in _instantiatedSlots)
		{
			slot.SetHighlighter(slot == _selectedCraftingOptonSlot);
		}
	}

	public void UpdateCraftability()
	{
		if (_playerInventory == null) _playerInventory = FindAnyObjectByType<PlayerInventory>();

		foreach (UICraftingOptionSlot slot in _instantiatedSlots)
		{

			foreach (CraftingRecipe.CraftMaterial material in slot.Recipe.MaterialsNeeded)
			{
				if (_playerInventory.CheckItemFromInventory(material.Item, material.Quantity) == false) 
				{
					//if (slot == _selectedCraftingOptonSlot) _selectedCraftingOptonSlot = null;

					slot.SetAvailability(false);
					break;
				}
				else
				{
					slot.SetAvailability(true);
				}
			}
		}
	}

	public void StartCraft()
	{
		if (_selectedCraftingOptonSlot == null) return;

		if (_playerInventory == null) _playerInventory = FindAnyObjectByType<PlayerInventory>();

		foreach (CraftingRecipe.CraftMaterial material in _selectedCraftingOptonSlot.Recipe.MaterialsNeeded)
		{
			_playerInventory.RemoveItemFromInventory(material.Item, material.Quantity);
		}

		Debug.Log(_selectedCraftingOptonSlot.Recipe);

		_playerInventory.AddItemToInventory(_selectedCraftingOptonSlot.Recipe.ResultingItem);


		foreach (CraftingRecipe.CraftMaterial material in _selectedCraftingOptonSlot.Recipe.MaterialsNeeded)
		{
			if (_playerInventory.CheckItemFromInventory(material.Item, material.Quantity) == false)
			{
				_selectedCraftingOptonSlot = null;
			}
		}
	}

	// PRIVATE METHODS

	private void SetUpInfoPanel()
	{

	}
}


[System.Serializable]
public struct CraftingRecipe
{
    public CraftMaterial[] MaterialsNeeded;

	[System.Serializable]
	public struct CraftMaterial
    {
        public ItemBase Item;
        public int Quantity;
    }

    public ItemBase ResultingItem;

    public float CraftingDuration;
}