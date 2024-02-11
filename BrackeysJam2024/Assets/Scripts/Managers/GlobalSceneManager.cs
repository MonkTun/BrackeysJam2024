using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalSceneManager : MonoBehaviour
{
    public static GlobalSceneManager Instance {  get; private set; }

	[SerializeField] private GameObject _loadPanel;

	private Coroutine _loadCoroutine;

	private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void OpenGame()
    {
        SceneManager.LoadScene("GameScene");
    }

	// Hello Henry below is from my SceneLoader

	public void OpenGameAsync(string sceneName) 
	{
		if (_loadCoroutine == null) return;

		_loadCoroutine = StartCoroutine(LoadAsyncScene(sceneName));
	}

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
