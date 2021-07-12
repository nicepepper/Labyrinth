using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State
{
    Begin,
    Pause,
    Expectation,
    Processing_AI,
    Door_Opening_By_Timer,
    Player_Opens_Door,
    Win,
    Lose,
    End
}

public class GameLogic : MonoBehaviour
{
    [SerializeField] private Timer _timer;
    [SerializeField] private PathInformation _pathInformation;
    [SerializeField] private FemaleDummyMovement _femaleDummyMovement;
    [SerializeField] private MapSettings _mapSettings;

    private State _state;

    private void Awake()
    {
        _state = State.Expectation;
    }

    private void Start()
    {
        _state = State.Begin;
    }

    private void Update()
    {
        switch (_state)
        {
            case State.Begin:
                SetInitialParameters();
                break;
            
            case State.Player_Opens_Door:    
                break;
            
            case State.Processing_AI:
                break;
            
            case State.Door_Opening_By_Timer:
                break;
            
            case State.Win:
                break;
            
            case State.Lose:
                break;
            
            case State.Expectation:
                break;
        }
    }

    private void SetInitialParameters()
    {
        
    }

    private void ProcessAI()
    {
        
    }

    private void OpenDoorOnTimer()
    {
        
    }
}
