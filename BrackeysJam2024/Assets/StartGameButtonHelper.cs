using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameButtonHelper : MonoBehaviour
{
    public void StartGame()
    {
        GlobalSceneManager.Instance.OpenGameAsync("GameScene");
    }
}
