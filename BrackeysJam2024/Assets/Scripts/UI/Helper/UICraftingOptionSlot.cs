using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UICraftingOptionSlot : MonoBehaviour
{
	public CraftingRecipe Recipe => _recipe;

	[SerializeField] private Image _resultingItemIconImg;
	[SerializeField] private TMP_Text _resultingItemText;
	[SerializeField] private Transform _materialsNeededContent;
	[SerializeField] private SimpleUIAccessor _simpleUIAccessor;
	[SerializeField] private GameObject _highlighter;
	[SerializeField] private GameObject _unavailableIndicator;
	[SerializeField] private Button _button;

	private CraftingUI _craftingUI;

	private CraftingRecipe _recipe;

	List<SimpleUIAccessor> _miniUI = new List<SimpleUIAccessor>();



	public void SetUp(CraftingRecipe recipe, CraftingUI craftingUI)
    {
		foreach (SimpleUIAccessor item in _miniUI)
		{
			Destroy(item.gameObject);
		}

		_miniUI = new List<SimpleUIAccessor>();



		_craftingUI = craftingUI;
		_recipe = recipe;

		_resultingItemIconImg.sprite = _recipe.ResultingItem.itemIcon;
		_resultingItemText.text = _recipe.ResultingItem.itemName;



		foreach (CraftingRecipe.CraftMaterial material in _recipe.MaterialsNeeded)
		{
			SimpleUIAccessor miniUIs = Instantiate(_simpleUIAccessor, _materialsNeededContent);

			_miniUI.Add(miniUIs);

			miniUIs.Image.sprite = material.Item.itemIcon;
			miniUIs.Text.text = material.Quantity.ToString();
		}
	}
	public void Select()
	{
		_craftingUI.SelectItem(this);
	}

	public void SetHighlighter(bool value)
	{
		_highlighter.SetActive(value);
	}

	public void SetAvailability(bool value)
	{
		_unavailableIndicator.SetActive(!value);
		_button.interactable = value;
	}
}
