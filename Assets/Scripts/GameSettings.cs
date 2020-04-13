using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    //----------RED--------------
    public void SetRedHumanType(bool on)
    {
        if(on) SaveSettings.players[0] = "HUMAN";
    }

    public void SetRedCpuType(bool on)
    {
        if(on) SaveSettings.players[0] = "CPU";
    }

    public void SetRedNoPlayerType(bool on)
    {
        if(on) SaveSettings.players[0] = "NO_PLAYER";
    }

    //----------GREEN--------------
    public void SetGreenHumanType(bool on)
    {
        if(on) SaveSettings.players[1] = "HUMAN";
    }

    public void SetGreenCpuType(bool on)
    {
        if(on) SaveSettings.players[1] = "CPU";
    }

    public void SetGreenNoPlayerType(bool on)
    {
        if(on) SaveSettings.players[1] = "NO_PLAYER";
    }

    //----------YELLOW--------------
    public void SetYellowHumanType(bool on)
    {
        if(on) SaveSettings.players[2] = "HUMAN";
    }

    public void SetYellowCpuType(bool on)
    {
        if(on) SaveSettings.players[2] = "CPU";
    }

    public void SetYellowNoPlayerType(bool on)
    {
        if(on) SaveSettings.players[2] = "NO_PLAYER";
    }

    //----------BLUE--------------
    public void SetBlueHumanType(bool on)
    {
        if(on) SaveSettings.players[3] = "HUMAN";
    }

    public void SetBlueCpuType(bool on)
    {
        if(on) SaveSettings.players[3] = "CPU";
    }

    public void SetBlueNoPlayerType(bool on)
    {
        if(on) SaveSettings.players[3] = "NO_PLAYER";
    }
}

public static class SaveSettings
{
    //RED GREEN YELLOW BLUE
    public static string[] players = new string[4];

    public static string[] playerNames = new string[4];

    public static string[] winners = new string[3] { "Not Applicable", "Not Applicable", "Not Applicable" };
}
