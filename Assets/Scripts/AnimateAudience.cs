using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateAudience : MonoBehaviour
{
    Animation animation;
    List<AudienceDolls> audienceDolls = new List<AudienceDolls>();

    void Start()
    {
        int childCount = transform.childCount;
        for(int i = 0; i < childCount; i++)
        {
            audienceDolls.Add(transform.GetChild(i).GetComponent<AudienceDolls>());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayAnimation()
    {
      for(int i = 0; i < audienceDolls.Count; i++)
      {
          switch(Random.Range(0,6)){
              case 0:
                audienceDolls[i].SetAnimation("applause");
              break;
              case 1:
                audienceDolls[i].SetAnimation("applause");
              break;
              case 2:
                audienceDolls[i].SetAnimation("applause2");
              break;
              case 3:
                audienceDolls[i].SetAnimation("celebration2");
              break;
              case 4:
                audienceDolls[i].SetAnimation("celebration2");
              break;
              case 5:
                audienceDolls[i].SetAnimation("celebration3");
              break;
          }
      }
    }
}
