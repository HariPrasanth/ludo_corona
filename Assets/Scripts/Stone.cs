using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public int stoneId;
    public int stoneIndividualId;
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

    public AudioSource stoneSound;
    public AudioSource cutSound;
    public AudioSource homeSound;

    int routePosition;
    int startNodeIndex;

    [Header("BOOLS")]
    bool isOut;
    bool isMoving;
    bool hasTurn;

    [Header("SELECTOR")]
    public GameObject selector;
    public GameObject highLighter;

    //Arc
    float amplitude = 0.5f;
    float cTime = 0f;

    bool didCut = false;

    int steps; // DICE NUMBER
    int doneSteps; // DICE NUMBER

    void Start()
    {
      startNodeIndex = commonRoute.RequestPosition(startNode.gameObject.transform);
      CreateFullRoute();

      SetSelector(false);
    }

    public void SetNotActive(){
      MeshRenderer mr = transform.GetComponent<MeshRenderer>();
      mr.enabled = false;
      highLighter.SetActive(false);
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

      // if(goalNode == homeNode){
      //   fullRoute[fullRoute.Count - 1].stoneCount++;
      // }

      if(!goalNode.isSafe && goalNode.isTaken)
      {
        didCut = true;
        goalNode.stone[0].ReturnToBase();
        goalNode.stone.Clear();
        goalNode.stoneIds.Clear();
        goalNode.stoneCount--;
      }

      if(currentNode.stoneCount > 0){
        currentNode.stoneCount--;
        int removePosition = 0;
        for(int i = 0; i < currentNode.stoneIds.Count ; i++){
            if(this.stoneIndividualId == currentNode.stoneIds[i]){
                removePosition = i;
                break;
            }
        }

        currentNode.resizeStone(currentNode.stone[removePosition]);
        currentNode.stone.RemoveAt(removePosition);
        currentNode.stoneIds.RemoveAt(removePosition);
        if (currentNode.isSafe)
        {
          switch(currentNode.stone.Count){
            case 1:
              currentNode.updateScale(1.0f);
            break;
            case 2:
              currentNode.updateScale(0.9f);
            break;
            case 3:
              currentNode.updateScale(0.8f);
            break;
            case 4:
              currentNode.updateScale(0.7f);
            break;
          }
        }
      }
      // currentNode.stone = null;
      currentNode.isTaken = false;

      goalNode.stone.Add(this);
      goalNode.stoneIds.Add(this.stoneIndividualId);
      goalNode.isTaken = true;
      goalNode.stoneCount++;

        if (goalNode.isSafe)
        {
            switch (goalNode.stone.Count)
            {
                case 1:
                    goalNode.updateScale(1.0f);
                    break;
                case 2:
                    goalNode.updateScale(0.9f);
                    break;
                case 3:
                    goalNode.updateScale(0.8f);
                    break;
                case 4:
                    goalNode.updateScale(0.7f);
                    break;
            }
        }

      currentNode = goalNode;
      goalNode = null;

      //report

      if(WinCondition())
      {
          GameManager.instance.ReportWinning();
      }

      if(diceNumber < 6 && !didCut)
      {
        GameManager.instance.state = GameManager.States.SWITCH_PLAYER;
      }else{
        GameManager.instance.state = GameManager.States.ROLL_DICE;
      }
      didCut = false;
      isMoving = false;
    }

    bool MoveToNextNode(Vector3 goalPos, float speed)
    {
      cutSound.Play();
      return goalPos != (transform.position = Vector3.MoveTowards(transform.position, goalPos, speed * Time.deltaTime));
    }

    bool MoveInArcToNextNode(Vector3 startPos, Vector3 goalPos, float speed)
    {
      cTime += speed * Time.deltaTime;
      Vector3 myPosition = Vector3.Lerp(startPos, goalPos, cTime);
      myPosition.y += amplitude * Mathf.Sin(Mathf.Clamp01(cTime) * Mathf.PI);
      stoneSound.Play();
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
        while(MoveInArcToNextNode(startPos, nextPos, 8f)){yield return null;}
        yield return new WaitForSeconds(0.05f);
        cTime = 0;
        steps--;
        doneSteps++;
        Debug.Log(steps+"");
      }

      goalNode = fullRoute[routePosition];

      // if(!goalNode.isSafe && goalNode.isTaken)
      // {
      //   //Cut
      //   goalNode.stone[0].ReturnToBase();
      // }

      goalNode.stone.Add(this);
      goalNode.stoneIds.Add(this.stoneIndividualId);
      goalNode.isTaken = true;
      goalNode.stoneCount++;

      if (goalNode.isSafe)
      {
        switch(goalNode.stone.Count){
          case 1:
            goalNode.updateScale(1.0f);
          break;
          case 2:
            goalNode.updateScale(0.9f);
          break;
          case 3:
            goalNode.updateScale(0.8f);
          break;
          case 4:
            goalNode.updateScale(0.7f);
          break;
        }
      }

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

      if(fullRoute[tempPos].isHome || fullRoute[tempPos].isSafe)
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
          if(stoneID == fullRoute[tempPos].stone[0].stoneId)
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
        if(fullRoute[fullRoute.Count - 1].stoneCount == 4)
        {
            homeSound.Play();
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
