using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemSlot : MonoBehaviour
{
    [SerializeField] private Image _iconImg;
	[SerializeField] private TMP_Text _countTxt;
	[SerializeField] private Image _highlighter;

	public void SetIcon(Sprite icon)
	{
		_iconImg.sprite = icon;
	}

	public void SetCount(int count)
	{
		_countTxt.text = count.ToString();
	}

	public void SetHighlighter(bool enable)
	{
		_highlighter.gameObject.SetActive(enable);
	}
}
