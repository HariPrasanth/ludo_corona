using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

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
