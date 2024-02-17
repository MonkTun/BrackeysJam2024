using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameButtonHelper : MonoBehaviour
{
    public string _mainScene;

    public void StartGame()
    {
        GlobalSceneManager.Instance.OpenGameAsync(_mainScene);
    }
}
