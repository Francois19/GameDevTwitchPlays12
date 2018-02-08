﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerManager), typeof(TerritoryManager), typeof(FactionManager))]
public class PhysicsManager : MonoBehaviour
{
    private TerritoryManager territoryManager;
    private PlayerManager playerManager;

    private FactionManager factionManager;
    public delegate void MyEndGameTimer(bool endGame, Faction faction);

    private static MyEndGameTimer m_onEndGameTimer;
    public float endTime = 1200f;

    private Timer timerRed;
    private Timer timerBlue;
    private Timer timerGreen;
    private Timer timerYellow;

    private void Awake()
    {
        territoryManager = gameObject.GetComponent<TerritoryManager>();
        playerManager = gameObject.GetComponent<PlayerManager>();
        factionManager= gameObject.GetComponent<FactionManager>();
        playerManager.m_territoryManager = territoryManager;
        AddEndGameTimerListener(EndGame);
        Timer timerGame = gameObject.AddComponent<Timer>();
        LaunchGameTimer(endTime, timerGame);
        timerGame.StartTimer();
        Territory.AddPlayerListener(FactionWins);
        Timer timerRed = gameObject.AddComponent<Timer>();
        Timer timerBlue = gameObject.AddComponent<Timer>();
        Timer timerGreen = gameObject.AddComponent<Timer>();
        Timer timerYellow = gameObject.AddComponent<Timer>();
    }

    private void EndGame(bool endGame, Faction faction)
    {
        Debug.Log("EndGame called, they want their code back !" + faction.NumFaction);
    }

    // Use this for initialization
    void Start ()
    {
        StartGame();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void StartGame()
    {
        territoryManager.GameStart();
    }

    public void CreatePlayer(string playerName)
    {
        playerManager.CreatePlayer(name);
    }

    public void SetCommandFromPlayer(string name, string command)    
    {
        var p = playerManager.GetPlayer(name);
        if (p == null)
        {
            p=playerManager.CreatePlayer(name);
            playerManager.DoAction(command, p);
        }
        else
        {
            playerManager.DoAction(command, p);
        }
    }

    public static void AddEndGameTimerListener(MyEndGameTimer timer)
    {
        m_onEndGameTimer += timer;

    }
    public static void RemoveEndGameTimeListener(MyEndGameTimer timer)
    {
        m_onEndGameTimer -= timer;

    }

    public static void NotifyEndGame()
    {
        Faction faction = WinningFaction();
        m_onEndGameTimer(true, faction);
    }

    public static void LaunchGameTimer(float seconds, Timer timer)
    {
        timer.LoadTimer(seconds, NotifyEndGame);
    }

    private static Faction WinningFaction()
    {
        int winningByInt = Mathf.Max(FactionManager.RED.NbrTerritories,
                    FactionManager.BLUE.NbrTerritories,
                    FactionManager.GREEN.NbrTerritories,
                    FactionManager.YELLOW.NbrTerritories);

        if (FactionManager.RED.NbrTerritories == winningByInt)
        {
            return FactionManager.RED;
        }
        if (FactionManager.BLUE.NbrTerritories == winningByInt)
        {
            return FactionManager.BLUE;
        }
        if (FactionManager.GREEN.NbrTerritories == winningByInt)
        {
            return FactionManager.GREEN;
        }
        if (FactionManager.YELLOW.NbrTerritories == winningByInt)
        {
            return FactionManager.YELLOW;
        }

        return null;
    }

    private void PlayerIsOnTerritory(Territory territory, Player player)
    {
        switch (player.Faction.NumFaction)
        {
            case 1:
                if (TeamHasGlassesInCenter(player.Faction))
                {
                    timerRed.StartTimer();
                }
                else
                {
                    timerRed.PauseTimer();
                }
                break;

            case 2:
                if (TeamHasGlassesInCenter(player.Faction))
                {
                    timerBlue.StartTimer();
                }
                else
                {
                    timerRed.PauseTimer();
                }
                break;

            case 3:
                if (TeamHasGlassesInCenter(player.Faction))
                {
                    timerGreen.StartTimer();
                }
                else
                {
                    timerRed.PauseTimer();
                }
                break;

            case 4:
                if (TeamHasGlassesInCenter(player.Faction))
                {
                    timerYellow.StartTimer();
                }
                else
                {
                    timerRed.PauseTimer();
                }
                break;
        }
    }

    private bool TeamHasGlassesInCenter(Faction faction)
    {
        bool fillsConditions = false;

        for (int i = 0; i < faction.ListPlayer.Count; i++)
        {
            if (faction.ListPlayer[i].HasGlasses && faction.ListPlayer[i].CurrentTerritory.IsCenter)
            {
                fillsConditions = true;
                i = faction.ListPlayer.Count;
            }
        }

        return fillsConditions;
    }

    private void FactionWins(Territory territory, Player player)
    {
        Debug.Log("This faction wins !" + territory.FactionNum);
    }
}
