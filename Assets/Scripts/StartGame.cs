﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    ArrayList leaderBoard = new ArrayList();
    protected bool isFirebaseInitialized = false;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0 ; i < SaveSettings.players.Length ; i++)
        {
            if(i == 0){
              SaveSettings.players[i] = "HUMAN";
              SaveSettings.playerNames[i] = "RED CPU";
            }
            if(i == 1){
                SaveSettings.players[i] = "CPU";
                SaveSettings.playerNames[i] = "GREEN CPU";
            }
            if(i == 2){
                SaveSettings.players[i] = "CPU";
                SaveSettings.playerNames[i] = "YELLOW CPU";
            }
            if(i == 3){
                SaveSettings.players[i] = "CPU";
                SaveSettings.playerNames[i] = "BLUE CPU";
            }
        }
    }

    public void StartTheGame(string sceneName)
    {
        int noPlayerCount = 0;
        for(int i = 0 ; i < SaveSettings.players.Length ; i++)
        {
            if(SaveSettings.players[i] == "NO_PLAYER")
            {
                noPlayerCount++;
            }
        }
        if(noPlayerCount <= 2)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
