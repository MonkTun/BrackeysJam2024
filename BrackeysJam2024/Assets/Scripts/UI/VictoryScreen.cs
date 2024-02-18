using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    private bool isHideComic;
    public GameObject VictoryComic;
    public GameObject crossfade;
    // Start is called before the first frame update
    void Start()
    {
        isHideComic = false;
        VictoryComic.SetActive(true);
        crossfade.SetActive(true);
        StartCoroutine(HideCrossfade());
    }

    private IEnumerator HideCrossfade()
    {
        yield return new WaitForSeconds(2);
        crossfade.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isHideComic)
        {
            isHideComic = true;
            VictoryComic.SetActive(false);
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
