using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMisc : MonoBehaviour
{
    private bool isPaused;
    
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                UIManager.Instance.ManageGameViews(UIManager.ViewState.Paused);
                Time.timeScale = 0f;
            }
            else
            {
                UIManager.Instance.ManageGameViews(UIManager.ViewState.GamePlay);
                Time.timeScale = 1f;
            }
            isPaused = !isPaused;
        }
    }
}
