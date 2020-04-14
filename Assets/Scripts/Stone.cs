using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public int stoneId;
    [Header("ROUTES")]
    public Route commonRoute;
    public Route finalRoute;

    public List<Node> fullRoute = new List<Node>();

    [Header("NODES")]
    public Node startNode;
    public Node baseNode;
    public Node homeNode;
    public Node currentNode;
    public Node goalNode;

    int routePosition;
    int startNodeIndex;

    [Header("BOOLS")]
    bool isOut;
    bool isMoving;
    bool hasTurn;

    [Header("SELECTOR")]
    public GameObject selector;

    //Arc
    float amplitude = 0.5f;
    float cTime = 0f;

    int steps; // DICE NUMBER
    int doneSteps; // DICE NUMBER

    void Start()
    {
      startNodeIndex = commonRoute.RequestPosition(startNode.gameObject.transform);
      CreateFullRoute();

      SetSelector(false);
    }

    void CreateFullRoute()
    {
      for(int i = 0 ; i < commonRoute.childNodeList.Count ; i++)
      {
        int tempPos = startNodeIndex + i;
        tempPos %= commonRoute.childNodeList.Count;
        fullRoute.Add(commonRoute.childNodeList[tempPos].GetComponent<Node>());
      }

      for(int i = 0 ; i < finalRoute.childNodeList.Count ; i++)
      {
        fullRoute.Add(finalRoute.childNodeList[i].GetComponent<Node>());
      }
    }

    IEnumerator Move(int diceNumber)
    {
      if(isMoving)
      {
        yield break;
      }

      isMoving = true;

      while(steps > 0)
      {
        routePosition++;

        Vector3 nextPos = fullRoute[routePosition].gameObject.transform.position;
        Vector3 startPos = fullRoute[routePosition-1].gameObject.transform.position;

        // while(MoveToNextNode(nextPos, 8f)){  yield return null;}

        while(MoveInArcToNextNode(startPos, nextPos, 8f)){yield return null;}

        yield return new WaitForSeconds(0.05f);
        cTime = 0;
        steps--;
        doneSteps++;
      }

      goalNode = fullRoute[routePosition];

      if(goalNode == homeNode){
        fullRoute[fullRoute.Count - 1].stoneCount++;
      }

      if(!goalNode.isSafe && goalNode.isTaken)
      {
        goalNode.stone.ReturnToBase();
      }

      currentNode.stone = null;
      currentNode.isTaken = false;
      if(currentNode.stoneCount > 0){
        currentNode.stoneCount--;
      }

      goalNode.stone = this;
      goalNode.isTaken = true;
      goalNode.stoneCount++;

      currentNode = goalNode;
      goalNode = null;

      //report

      if(WinCondition())
      {
          GameManager.instance.ReportWinning();
      }

      if(diceNumber < 6)
      {
        GameManager.instance.state = GameManager.States.SWITCH_PLAYER;
      }else{
        GameManager.instance.state = GameManager.States.ROLL_DICE;
      }

      isMoving = false;
    }

    bool MoveToNextNode(Vector3 goalPos, float speed)
    {
      return goalPos != (transform.position = Vector3.MoveTowards(transform.position, goalPos, speed * Time.deltaTime));
    }

    bool MoveInArcToNextNode(Vector3 startPos, Vector3 goalPos, float speed)
    {
      cTime += speed * Time.deltaTime;
      Vector3 myPosition = Vector3.Lerp(startPos, goalPos, cTime);
      myPosition.y += amplitude * Mathf.Sin(Mathf.Clamp01(cTime) * Mathf.PI);

      return goalPos != (transform.position = Vector3.Lerp(transform.position, myPosition, cTime));
    }

    public bool ReturnIsOut()
    {
      return isOut;
    }


    IEnumerator MoveOut()
    {
      if(isMoving)
      {
        yield break;
      }

      isMoving = true;

      while(steps > 0)
      {
        // routePosition++;
        Vector3 nextPos = fullRoute[routePosition].gameObject.transform.position;
        Vector3 startPos = baseNode.gameObject.transform.position;
        // while(MoveToNextNode(nextPos, 8f)){yield return null;}
        while(MoveInArcToNextNode(startPos, nextPos, 8f)){yield return null;}
        yield return new WaitForSeconds(0.05f);
        cTime = 0;
        steps--;
        doneSteps++;
        Debug.Log(steps+"");
      }

      goalNode = fullRoute[routePosition];

      if(!goalNode.isSafe && goalNode.isTaken)
      {
        //Cut
        goalNode.stone.ReturnToBase();
      }

      goalNode.stone = this;
      goalNode.isTaken = true;
      goalNode.stoneCount++;

      currentNode = goalNode;
      goalNode = null;

      GameManager.instance.state = GameManager.States.ROLL_DICE;
      isMoving = false;
    }

    public void LeaveBase()
    {
      steps = 1;
      isOut = true;
      routePosition = 0;
      StartCoroutine(MoveOut());
    }

    public bool CheckPossibleMove(int diceNumber)
    {
      int tempPos = routePosition + diceNumber;
      if(tempPos >= fullRoute.Count)
      {
        return false;
      }

      if(fullRoute[tempPos].isHome)
      {
          return true;
      }

      return !fullRoute[tempPos].isTaken;
    }

    public bool CheckPossibleKick(int stoneID , int diceNumber)
    {
      int tempPos = routePosition + diceNumber;
      if(tempPos >= fullRoute.Count)
      {
        return false;
      }

      if(!fullRoute[tempPos].isSafe && fullRoute[tempPos].isTaken)
      {
          if(stoneID == fullRoute[tempPos].stone.stoneId)
          {
            return false;
          }
          return true;
      }
      return false;
    }

    public void StartTheMove(int diceNumber)
    {
      steps = diceNumber;
      StartCoroutine(Move(diceNumber));
    }

    public void ReturnToBase()
    {
        StartCoroutine(Return());
    }

    IEnumerator Return()
    {
      GameManager.instance.ReportTurnPossible(false);
      routePosition = 0;
      currentNode = null;
      goalNode = null;
      isOut = false;
      doneSteps = 0;
      Vector3 baseNodePos = baseNode.gameObject.transform.position;
      while(MoveToNextNode(baseNodePos, 100f)){yield return null;}
      GameManager.instance.ReportTurnPossible(true);
    }

    bool WinCondition()
    {
        // int completeCount = 0;
        // for(int i = 0 ; i < finalRoute.childNodeList.Count ; i++)
        // {
        //   if(finalRoute.childNodeList[i].GetComponent<Node>().isTaken)
        //   {
        //     completeCount++;
        //   }
        // }
        if(fullRoute[fullRoute.Count - 1].stoneCount == 8)
        {
            Debug.Log("Complete Count "+fullRoute[fullRoute.Count - 1].stoneCount);
            return true;
        }
        return false;
    }

    //------------Human-------------

    public void SetSelector(bool on)
    {
        selector.SetActive(on);
        hasTurn = on;
    }

    void OnMouseDown()
    {
        if(hasTurn)
        {
            if(!isOut)
            {
                LeaveBase();
            }
            else{
                StartTheMove(GameManager.instance.rolledHumanDice);
            }
            GameManager.instance.DeactivateAllSelectors();
        }
    }
}
