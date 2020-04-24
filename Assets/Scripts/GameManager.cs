using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [System.Serializable]
    public class Entity
    {
      public string playerName;
      public Stone[] myStones;
      public bool hasTurn;
      public enum PlayerTypes
      {
        HUMAN,
        CPU,
        NO_PLAYER
      }
      public PlayerTypes playerType;
      public bool hasWon;
    }

    public List<Entity> playerList = new List<Entity>();
    public List<GameObject> playerMedals = new List<GameObject>();

    public enum States
    {
      WAITING,
      ROLL_DICE,
      SWITCH_PLAYER
    }

    public States state;


    public int activePlayer;
    bool switchingPlayer;
    bool turnPossible = true;
    public Text txt;

    //Human input
    public GameObject rollButtonRed;
    public GameObject rollButtonGreen;
    public GameObject rollButtonYellow;
    public GameObject rollButtonBlue;

    public GameObject turnButtonRed;
    public GameObject turnButtonGreen;
    public GameObject turnButtonYellow;
    public GameObject turnButtonBlue;
    // public GameObject audience;

    [HideInInspector]public int rolledHumanDice;
    public Dice dice;

    void Awake()
    {
      instance = this;
      for(int i = 0 ; i<playerList.Count ; i++)
      {
          if(SaveSettings.players[i] == "HUMAN")
          {
              playerList[i].playerType = Entity.PlayerTypes.HUMAN;
              if(i == 0)
                  playerList[i].playerName = "RED HUMAN";
              if(i == 1)
                  playerList[i].playerName = "GREEN HUMAN";
              if(i == 2)
                  playerList[i].playerName = "YELLOW HUMAN";
              if(i == 3)
                  playerList[i].playerName = "BLUE HUMAN";
          }

          if(SaveSettings.players[i] == "CPU")
          {
              playerList[i].playerType = Entity.PlayerTypes.CPU;
              if(i == 0)
                  playerList[i].playerName = "RED CPU";
              if(i == 1)
                  playerList[i].playerName = "GREEN CPU";
              if(i == 2)
                  playerList[i].playerName = "YELLOW CPU";
              if(i == 3)
                  playerList[i].playerName = "BLUE CPU";
          }

          if(SaveSettings.players[i] == "NO_PLAYER")
          {
              playerList[i].playerType = Entity.PlayerTypes.NO_PLAYER;
              for(int j = 0; j< 4; j++)
              {
                playerList[i].myStones[j].SetNotActive();
              }
              if(i == 0)
                  playerList[i].playerName = "RED";
              if(i == 1)
                  playerList[i].playerName = "GREEN";
              if(i == 2)
                  playerList[i].playerName = "YELLOW";
              if(i == 3)
                  playerList[i].playerName = "BLUE";
          }
      }

      for(int i = 0 ; i<playerMedals.Count ; i++)
      {
          playerMedals[i].SetActive(false);
      }
      txt.enabled = false;
    }

    void Start()
    {
        ActivateButton(false);

        int randomPlayer = Random.Range(0, playerList.Count);
        activePlayer = randomPlayer;
        // Info.instance.ShowMessage(playerList[activePlayer].playerName +" starts first!");
        Info.instance.ShowMessage("Roll a 6 to Move out");
    }

    int click = 0;
    IEnumerator ClickTime()
     {
         yield return new WaitForSeconds(1f);
         click = 0;
     }
    void Update()
    {
      ActivateTurn(true);
      if(!playerList[activePlayer].hasWon)
      {
        if(playerList[activePlayer].playerType == Entity.PlayerTypes.CPU)
        {
          switch(state)
          {
            case States.ROLL_DICE:
              {
                  if(turnPossible)
                  {
                    StartCoroutine(RollDiceDelay());
                    state = States.WAITING;
                  }
              }
            break;

            case States.WAITING:
              {

              }
            break;

            case States.SWITCH_PLAYER:
              {
                if(turnPossible)
                {
                    StartCoroutine(SwitchPlayer());
                    state = States.WAITING;
                }
              }
            break;
          }
        }

        if(playerList[activePlayer].playerType == Entity.PlayerTypes.HUMAN)
        {
          switch(state)
          {
            case States.ROLL_DICE:
              {
                  if(turnPossible)
                  {
                    //Deactive highligh
                    ActivateButton(true);
                    state = States.WAITING;
                  }
              }
            break;

            case States.WAITING:
              {

              }
            break;

            case States.SWITCH_PLAYER:
              {
                if(turnPossible)
                {
                    //Deactivate highligh
                    ActivateButton(false);
                    StartCoroutine(SwitchPlayer());
                    state = States.WAITING;
                }
              }
            break;
          }
        }

        if(playerList[activePlayer].playerType == Entity.PlayerTypes.NO_PLAYER)
        {
          if(turnPossible)
          {
              //Deactivate highligh
              ActivateButton(false);
              StartCoroutine(SwitchPlayer());
              state = States.WAITING;
          }
        }
      } else {
          //Deactivate highligh
          ActivateButton(false);
          StartCoroutine(SwitchPlayer());
          state = States.WAITING;
      }

      for(int i = 0 ; i<playerList.Count ; i++)
      {
          if(playerList[i].hasWon)
          {
              playerMedals[i].SetActive(true);
          }else{
              playerMedals[i].SetActive(false);
          }
      }
         if (Input.GetKeyDown(KeyCode.Escape))
         {
              showToast("Press back again to exit", 1);
             click++;
             StartCoroutine(ClickTime());

               if (click > 1)
               {
                   if (Application.platform == RuntimePlatform.Android)
                          {
                            AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                            activity.Call<bool>("moveTaskToBack", true);
                          }
                          else
                          {
                              Application.Quit();
                          }
               }
         }
    }

    void showToast(string text,
        int duration)
    {
        StartCoroutine(showToastCOR(text, duration));
    }

    private IEnumerator showToastCOR(string text,
        int duration)
    {
        // Color orginalColor = Color.white;

        txt.text = text;
        txt.enabled = true;

        //Fade in
        yield return fadeInAndOut(txt, true, 0.5f);

        //Wait for the duration
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            yield return null;
        }

        //Fade out
        yield return fadeInAndOut(txt, false, 0.5f);

        txt.enabled = false;
        // txt.color = orginalColor;
    }

    IEnumerator fadeInAndOut(Text targetText, bool fadeIn, float duration)
    {
        //Set Values depending on if fadeIn or fadeOut
        float a, b;
        if (fadeIn)
        {
            a = 0f;
            b = 1f;
        }
        else
        {
            a = 1f;
            b = 0f;
        }

        Color currentColor = Color.white;
        float counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);

            targetText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }
    }

    void CPUDice()
    {
          dice.RollDice();
    }

    public void PlayAudienceAnimation()
    {
        // AnimateAudience animateAudience = audience.GetComponent<AnimateAudience>();
        // animateAudience.PlayAnimation();
    }

    public void RollDice(int _diceNumber)
    {
      PlayAudienceAnimation();
      int diceNumber = _diceNumber;
      if(playerList[activePlayer].playerType == Entity.PlayerTypes.CPU)
      {
          if(diceNumber == 6)
          {
              CheckStartNode(diceNumber);
          }

          if(diceNumber < 6)
          {
              MoveAStone(diceNumber);
          }
      }

      if(playerList[activePlayer].playerType == Entity.PlayerTypes.HUMAN)
      {
          rolledHumanDice = _diceNumber;
          HumanRollDice();
      }

      // Debug.Log("Dice Rolled is : "+diceNumber);
      // Info.instance.ShowMessage(playerList[activePlayer].playerName + " has rolled "+ diceNumber);
    }

    IEnumerator RollDiceDelay()
    {
      yield return new WaitForSeconds(0.05f);
      // RollDice();
      CPUDice();
    }

    void CheckStartNode(int diceNumber)
    {
      bool startNodeFull = false;
      // for(int i = 0 ; i < playerList[activePlayer].myStones.Length; i++)
      // {
      //     if(playerList[activePlayer].myStones[i].currentNode == playerList[activePlayer].myStones[i].startNode)
      //     {
      //       startNodeFull = true;
      //       break;
      //     }
      // }

      if(startNodeFull)
      {
        //move a stone
        MoveAStone(diceNumber);
        // Debug.Log("The start node is full");
      }
      else
      {

        for(int i = 0 ; i < playerList[activePlayer].myStones.Length; i++)
        {
          if(!playerList[activePlayer].myStones[i].ReturnIsOut())
          {
            //Leave the base
            playerList[activePlayer].myStones[i].LeaveBase();
            state = States.WAITING;
            return;
          }
        }

        MoveAStone(diceNumber);
      }
    }

    void MoveAStone(int diceNumber)
    {
        List<Stone> movableStones = new List<Stone>();
        List<Stone> moveKickStones = new List<Stone>();

        for(int i = 0 ; i<playerList[activePlayer].myStones.Length ; i++)
        {
              if(playerList[activePlayer].myStones[i].ReturnIsOut())
              {
                if(playerList[activePlayer].myStones[i].CheckPossibleKick(playerList[activePlayer].myStones[i].stoneId, diceNumber))
                {
                    moveKickStones.Add(playerList[activePlayer].myStones[i]);
                    continue;
                }

                if(playerList[activePlayer].myStones[i].CheckPossibleMove(diceNumber))
                {
                    movableStones.Add(playerList[activePlayer].myStones[i]);
                }
              }
        }

        //Perform kick
        if(moveKickStones.Count > 0)
        {
            int num = Random.Range(0, moveKickStones.Count);
            moveKickStones[num].StartTheMove(diceNumber);
            state = States.WAITING;
            return;
        }

        //Perform move
        if(movableStones.Count > 0)
        {
            int num = Random.Range(0, movableStones.Count);
            movableStones[num].StartTheMove(diceNumber);
            state = States.WAITING;
            return;
        }

        state = States.SWITCH_PLAYER;
        // Debug.Log("Should switch player");
    }

    IEnumerator SwitchPlayer()
    {
      if(switchingPlayer)
      {
          yield break;
      }

      switchingPlayer = true;
      yield return new WaitForSeconds(0.05f);
      setNextActivePlayer();
      switchingPlayer = false;
    }

    void setNextActivePlayer()
    {
        activePlayer++;
        activePlayer %= playerList.Count;

        int available = 0;
        for(int i = 0 ; i < playerList.Count ; i++)
        {
            if(!playerList[i].hasWon && playerList[i].playerType != Entity.PlayerTypes.NO_PLAYER)
            {
                available++;
            }
        }
        // Debug.Log("Game Manager Next Active Player!!!" + available);
        if(playerList[activePlayer].hasWon && available > 2)
        {
            setNextActivePlayer();
            return;
        }else if(available < 2)
        {
          //GAME OVER SCREEN
          Debug.Log("Game Over!!!");
          // Info.instance.ShowMessage("Game Over!!!");
          SceneManager.LoadScene("GameOver");
          state = States.WAITING;
          return;
        }

        // Info.instance.ShowMessage(playerList[activePlayer].playerName + " has got the turn!");
        state = States.ROLL_DICE;
    }

    int restartClick = 0;
    IEnumerator RestartClickTime()
     {
         yield return new WaitForSeconds(1f);
         restartClick = 0;
     }

    public void RestartGame()
    {
      showToast("Press restart again to exit", 1);
      restartClick++;
      StartCoroutine(RestartClickTime());
       if (restartClick > 1)
       {
           SceneManager.LoadScene("Menu");
       }
    }

    public void ReportTurnPossible(bool possible)
    {
        turnPossible = possible;
    }

    public void ReportWinning()
    {
        for(int i = 0; i < SaveSettings.winners.Length ; i++)
        {
            if(SaveSettings.winners[i] == "Not Applicable" && !playerList[activePlayer].hasWon)
            {
                Debug.Log("Player Won : "+playerList[activePlayer].playerName);
                SaveSettings.winners[i] = playerList[activePlayer].playerName;
                break;
            }
        }
        // Info.instance.ShowMessage(playerList[activePlayer].playerName + " player has completed!!!");
        playerList[activePlayer].hasWon = true;
    }

    void ActivateTurn(bool on)
    {
        switch(activePlayer)
        {
          case 0:
            turnButtonRed.SetActive(on);
            turnButtonGreen.SetActive(false);
            turnButtonYellow.SetActive(false);
            turnButtonBlue.SetActive(false);
          break;
          case 1:
            turnButtonGreen.SetActive(on);
            turnButtonRed.SetActive(false);
            turnButtonYellow.SetActive(false);
            turnButtonBlue.SetActive(false);
          break;
          case 2:
            turnButtonYellow.SetActive(on);
            turnButtonGreen.SetActive(false);
            turnButtonRed.SetActive(false);
            turnButtonBlue.SetActive(false);
          break;
          case 3:
            turnButtonBlue.SetActive(on);
            turnButtonYellow.SetActive(false);
            turnButtonGreen.SetActive(false);
            turnButtonRed.SetActive(false);
          break;
        }
    }

    //-------------------------------Human input---------------

    void ActivateButton(bool on)
    {
        switch(activePlayer)
        {
          case 0:
            rollButtonRed.SetActive(on);
            rollButtonGreen.SetActive(false);
            rollButtonYellow.SetActive(false);
            rollButtonBlue.SetActive(false);
          break;
          case 1:
            rollButtonGreen.SetActive(on);
            rollButtonRed.SetActive(false);
            rollButtonYellow.SetActive(false);
            rollButtonBlue.SetActive(false);
          break;
          case 2:
            rollButtonYellow.SetActive(on);
            rollButtonGreen.SetActive(false);
            rollButtonRed.SetActive(false);
            rollButtonBlue.SetActive(false);
          break;
          case 3:
            rollButtonBlue.SetActive(on);
            rollButtonYellow.SetActive(false);
            rollButtonGreen.SetActive(false);
            rollButtonRed.SetActive(false);
          break;
        }
    }

    public void DeactivateAllSelectors()
    {
      for(int i = 0 ; i < playerList.Count ; i++)
      {
          for(int j = 0 ; j < playerList[i].myStones.Length ; j++)
          {
              playerList[i].myStones[j].SetSelector(false);
          }
      }
    }

    public void HumanRoll()
    {
        dice.RollDice();
        ActivateButton(false);
    }

    //Roll dice button function
    public void HumanRollDice()
    {
        // rolledHumanDice = Random.Range(1, 7);
        // rolledHumanDice = 6;

        List<Stone> movableStones = new List<Stone>();
        //Check start node full
        bool startNodeFull = false;
        // for(int i = 0 ; i < playerList[activePlayer].myStones.Length; i++)
        // {
        //     if(playerList[activePlayer].myStones[i].currentNode == playerList[activePlayer].myStones[i].startNode)
        //     {
        //       startNodeFull = true;
        //       break;
        //     }
        // }

        if(rolledHumanDice < 6)
        {
            movableStones.AddRange(PossibleStones());
        }

        if(rolledHumanDice == 6 && !startNodeFull)
        {
            //Inside base check
            for(int i = 0 ; i < playerList[activePlayer].myStones.Length; i++)
            {
              if(!playerList[activePlayer].myStones[i].ReturnIsOut())
              {
                  movableStones.Add(playerList[activePlayer].myStones[i]);
              }
            }

            //outside check
            movableStones.AddRange(PossibleStones());
        }
        else if(rolledHumanDice == 6 && startNodeFull)
        {
            movableStones.AddRange(PossibleStones());
        }

        //activate all possible
        if(movableStones.Count > 0)
        {

          if(movableStones.Count == 1)
          {
              MoveAStone(rolledHumanDice);
          }else{
            for(int i = 0 ; i < movableStones.Count; i++)
            {
                movableStones[i].SetSelector(true);
            }
          }
        }
        else
        {
            state = States.SWITCH_PLAYER;
        }
    }

    List<Stone> PossibleStones()
    {
        List<Stone> tempList = new List<Stone>();
        for(int i = 0 ; i < playerList[activePlayer].myStones.Length; i++)
        {
            //Make sure he is out already
            if(playerList[activePlayer].myStones[i].ReturnIsOut())
            {
                if(playerList[activePlayer].myStones[i].CheckPossibleKick(playerList[activePlayer].myStones[i].stoneId, rolledHumanDice))
                {
                    tempList.Add(playerList[activePlayer].myStones[i]);
                    continue;
                }

                if(playerList[activePlayer].myStones[i].CheckPossibleMove(rolledHumanDice))
                {
                    tempList.Add(playerList[activePlayer].myStones[i]);
                }
            }
        }

        return tempList;
    }
}
