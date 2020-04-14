using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public bool isTaken;
    public List<Stone> stone = new List<Stone>();
    public List<int> stoneIds = new List<int>();
    public bool isSafe;
    public bool isHome;
    public int stoneCount;

    public void updateScale(float scaleSize)
    {
        Debug.Log(gameObject.transform.position);
        Debug.Log(gameObject.transform.position + gameObject.transform.localScale);
        foreach(Stone singleStone in stone)
        {
            singleStone.transform.localScale = new Vector3(scaleSize,scaleSize,scaleSize);
        }
    }
}
