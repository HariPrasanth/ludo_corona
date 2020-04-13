using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
  #if UNITY_IOS || UNITY_ANDROID
    public float speed;
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 2)
          transform.Rotate(0, speed * Time.deltaTime, 0);
        else if (Input.touchCount >= 3)
          transform.Rotate(0, -speed * Time.deltaTime, 0);
    }
  #endif
}
