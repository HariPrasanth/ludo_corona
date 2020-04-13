using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceDolls : MonoBehaviour
{
    Animation animation;

    void Start()
    {
        animation = GetComponent<Animation>();
    }
    public void SetAnimation(string animationName)
    {
      animation.clip = animation.GetClip(animationName);
      animation.Play();
    }
}
