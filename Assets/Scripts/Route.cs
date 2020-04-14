using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    Transform[] childNodes;
    public List<Transform> childNodeList = new List<Transform>();
    void Start()
    {
      FillNodes();
    }

    void OnDrawGizmos()
    {
      Gizmos.color = Color.black;
      FillNodes();
      for(int i = 0; i < childNodeList.Count ; i++)
      {
        Vector3 pos = childNodeList[i].position;
        if(i > 0)
        {
          Vector3 prev = childNodeList[i - 1].position;
          Gizmos.DrawLine(prev, pos);
        }
      }
    }

    void FillNodes()
    {
        childNodeList.Clear();

        Node[] nodes = GetComponentsInChildren<Node>();

        childNodes = new Transform[nodes.Length];

        for(int i = 0; i < nodes.Length; i++)
        {
            childNodes[i] = nodes[i].GetComponent<Transform>();
        }

        foreach(Transform child in childNodes)
        {
        if(child != this.transform)
        {
            childNodeList.Add(child);
        }
        }
    }

    public int RequestPosition(Transform nodeTransform)
    {
      return childNodeList.IndexOf(nodeTransform);
    }
}
