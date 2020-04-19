using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Text first, second, third;
    public AudioSource gameOverAudio;
    // Start is called before the first frame update
    void Start()
    {
        gameOverAudio.Play();
        first.text = "1st : "+SaveSettings.winners[0];
        second.text = "2nd : "+SaveSettings.winners[1];
        third.text = "3rd : "+SaveSettings.winners[2];
    }

    public void BackButton(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void Update()
    {
      if (Input.GetKeyUp(KeyCode.Escape))
         {
             if (Application.platform == RuntimePlatform.Android)
             {
                Application.Quit();
             }
             else
             {
                 Application.Quit();
             }
         }
    }
}
