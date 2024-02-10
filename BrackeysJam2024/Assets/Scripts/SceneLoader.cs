using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Will be present in everyscene
/// </summary>
public class SceneLoader : MonoBehaviour
{
	// PUBLIC MEMBERS
	public static SceneLoader Instance { get; private set; }

	// PRVATE MEMBERS

	[SerializeField] private GameObject _loadPanel;

	private Coroutine _loadCoroutine;

	// MONOBEHAVIOUR

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

		DontDestroyOnLoad(gameObject);
	}

	// PUBLIC METHODS

	public void LoadScene(string sceneName)
	{
		if (_loadCoroutine == null) return;

		_loadCoroutine = StartCoroutine(LoadAsyncScene(sceneName));
	}

	// PRIVATE METHODS

	private IEnumerator LoadAsyncScene(string sceneName)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

		_loadPanel.SetActive(true);

		//TODO Animation	

		while (!asyncLoad.isDone)
		{
			yield return null;
		}

		_loadPanel.SetActive(false);

		_loadCoroutine = null;
	}
}
