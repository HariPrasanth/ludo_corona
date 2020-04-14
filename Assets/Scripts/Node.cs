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
        List<Vector3> stonePositions = new List<Vector3>();
        if (stone.Count > 1)
            stonePositions = GetPositions(stone.Count);

        Vector3 offsetPosition = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);

        int index = 0;
        foreach(Stone singleStone in stone)
        {
            singleStone.transform.localScale = new Vector3(scaleSize,scaleSize,scaleSize);
            if (stone.Count > 1)
                singleStone.transform.position = stonePositions[index++];
            else
              singleStone.transform.position = gameObject.transform.position;
        }
    }

    public void resizeStone(Stone singleStone){
        singleStone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    public float GetRectLength()
    {
        if (isSafe)
        {
            Transform bottomLeft = gameObject.transform.GetChild(0).transform;
            Transform topRight = gameObject.transform.GetChild(1).transform;

            return topRight.position.x - bottomLeft.position.x;
        }
        return 0;
    }

    // public void Update()
    // {
    //   if (isSafe && stone.Count >= 1)
    //   {
    //     Debug.Log("Cojming in" + stone.Count);
    //     switch(stone.Count){
    //       case 1:
    //         updateScale(1.0f);
    //       break;
    //       case 2:
    //         updateScale(0.9f);
    //       break;
    //       case 3:
    //         updateScale(0.8f);
    //       break;
    //       case 4:
    //         updateScale(0.7f);
    //       break;
    //     }
    //   }
    // }

    public List<Vector3> GetPositions(int elementsSize)
    {
        List<Vector3> returnVectors = new List<Vector3>();
        Transform bottomLeft = gameObject.transform.GetChild(0).transform;
        Transform topRight = gameObject.transform.GetChild(1).transform;

        switch (elementsSize)
        {
            case 2:
                {
                    float midPointX = (topRight.position.x - bottomLeft.position.x) / 4;
                    float midPointY = (topRight.position.y - bottomLeft.position.y) / 4;

                    Vector3 firstPosition = new Vector3(bottomLeft.position.x + midPointX, bottomLeft.position.y, bottomLeft.position.z + midPointY + 0.18f);
                    Vector3 secondPosition = new Vector3(topRight.position.x - midPointX, topRight.position.y, topRight.position.z + midPointY - 0.18f);

                    returnVectors.Add(firstPosition);
                    returnVectors.Add(secondPosition);

                    break;
                }
            case 3:
                {
                    float midPointX = (topRight.position.x - bottomLeft.position.x) / 4;
                    float midPointY = (topRight.position.y - bottomLeft.position.y) / 4;

                    Vector3 firstPosition = new Vector3(bottomLeft.position.x + midPointX, bottomLeft.position.y, bottomLeft.position.z + midPointY + 0.18f);
                    Vector3 secondPosition = new Vector3(bottomLeft.position.x + midPointX * 3, bottomLeft.position.y, bottomLeft.position.z + midPointY + 0.18f);
                    Vector3 thirdPosition = new Vector3(topRight.position.x - midPointX * 2, topRight.position.y, topRight.position.z + midPointY - 0.18f);

                    returnVectors.Add(firstPosition);
                    returnVectors.Add(secondPosition);
                    returnVectors.Add(thirdPosition);
                }
                break;
            case 4:
                {
                    float midPointX = (topRight.position.x - bottomLeft.position.x) / 4;
                    float midPointY = (topRight.position.y - bottomLeft.position.y) / 4;

                    Vector3 firstPosition = new Vector3(bottomLeft.position.x + midPointX, bottomLeft.position.y, bottomLeft.position.z + midPointY + 0.18f);
                    Vector3 secondPosition = new Vector3(bottomLeft.position.x + midPointX, bottomLeft.position.y, bottomLeft.position.z + (midPointY + 0.18f * 3));
                    Vector3 thirdPosition = new Vector3(topRight.position.x - midPointX, topRight.position.y, topRight.position.z - midPointY - 0.18f);
                    Vector3 fourthPosition = new Vector3(topRight.position.x - midPointX, topRight.position.y, topRight.position.z - (midPointY + 0.18f * 3));

                    returnVectors.Add(firstPosition);
                    returnVectors.Add(secondPosition);
                    returnVectors.Add(thirdPosition);
                    returnVectors.Add(fourthPosition);
                    break;
                }
        }
        return returnVectors;
    }
}
