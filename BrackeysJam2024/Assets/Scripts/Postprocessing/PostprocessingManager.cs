using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostprocessingManager : MonoBehaviour
{
    public GameObject gameplayPP;
    public GameObject nearDeathPP;
    public GameObject pausedPP;

    public static PostprocessingManager Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        GameplayPPOn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameplayPPOn()
    {
        gameplayPP.SetActive(true);
    }

    public void GameplayPPOff()
    {
        gameplayPP.SetActive(false);
    }

    public void NearDeathPPOff()
    {
        nearDeathPP.SetActive(false);
    }

    public void NearDeathPPOn()
    {
        nearDeathPP.SetActive(true);
    }

    public void PausedPPOn()
    {
        pausedPP.SetActive(true);
    }

    public void PausedPPOff()
    {
        pausedPP.SetActive(false);
    }
}
