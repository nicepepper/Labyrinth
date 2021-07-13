using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class RoomWithDoors
{
    public string roomName;
    public List<int> doorsId = new List<int>();
}

public enum State
{
    Begin,
    Pause,
    Expectation,
    Processing_AI,
    Door_Opening_By_Timer,
    Player_Opens_Door,
    Chasing,
    Win,
    Lose,
    End,
    Reset_State
}

public class GameLogic : MonoBehaviour
{
    [SerializeField] private Timer _timer;
    [SerializeField] private PathInformation _pathInformation;
    [SerializeField] private FemaleDummyMovement _femaleDummyMovement;
    [SerializeField] private MaleDummyMovement _maleDummyMovement;
    [SerializeField] private MapSettings _mapSettings;
    [SerializeField] private DoorLogic _doorLogic;

    private State _state;
    private List<RoomWithDoors> _roomsWithDoors = new List<RoomWithDoors>();

    private void Start()
    {
        InitializeRoomsWithTheirDoors();
        ResetState();
        
        _state = State.Begin;
    }

    private void Update()
    {
        switch (_state)
        {
            case State.Begin:
                ProcessStart();
                break;
            
            case State.Player_Opens_Door:
                ProcessPlayer();
                break;
            
            case State.Processing_AI:
                ProcessAI();
                break;
            
            case State.Door_Opening_By_Timer:
                ProcessOpenDoorOnTimer();
                break;

            case State.Expectation:
                ProcessExpectation();
                break;
            
            case State.Reset_State:
                break;
            case State.Chasing:
                ProcessChasing();
                break;
        }
    }
    
    private void InitializeRoomsWithTheirDoors()
    {
        _roomsWithDoors.Capacity = _mapSettings.vertices.Count;

        foreach (var vertex in _mapSettings.vertices)
        {
            RoomWithDoors el = new RoomWithDoors();
            el.roomName = vertex.Name;
            FindDoorsIdOfRoom(el.roomName, el.doorsId);
            
            _roomsWithDoors.Add(el);
        }
    }
    
    private void FindDoorsIdOfRoom(string roomNumber, List<int> list)
    {
        foreach (var adjacency in _mapSettings.adjacencies)
        {
            if (adjacency.FirstName == roomNumber || adjacency.SecondName == roomNumber)
            {
                list.Add(adjacency.Id);
            }
        }
    }

    private void ProcessStart()
    {
        if (_pathInformation.isDoorOpened)
        {
            _state = _timer.IsOver ? State.Door_Opening_By_Timer : State.Player_Opens_Door;
        }
    }

    private void ProcessAI()
    {
        _state = State.Expectation;
    }

    private void ProcessPlayer()
    {
        _pathInformation.isDoorOpened = false;
        _timer.Restart();
        _state = State.Chasing;
    }

    private void ProcessOpenDoorOnTimer()
    {
        _timer.Restart();
        
        if (_pathInformation.numberOfDoorsOnPathToEnemy == 1)
        {
            _doorLogic.OpenDoorById(GetDoorId(_pathInformation.playerLocation, _pathInformation.enemyLocation));
            _state = State.Chasing;
            return;
        }
        
        var rand = new Random();
        foreach (var roomWithDoors in _roomsWithDoors)
        {
            if (Equals(roomWithDoors.roomName, _pathInformation.playerLocation))
            {
                for (;;)
                {
                    var id = roomWithDoors.doorsId[rand.Next(0, roomWithDoors.doorsId.Count)];
                    if (!_doorLogic.IsActedDoorById(id))
                    {
                        _doorLogic.OpenDoorById(id);
                        break;
                    }
                }
            }
        }

        _state = State.Chasing;
    }

    private int GetDoorId(string firstRoom, string secondRoom)
    {
        foreach (var adjacency in _mapSettings.adjacencies)
        {
            if ((Equals(adjacency.FirstName, firstRoom) && Equals(adjacency.SecondName, secondRoom)) || (Equals(adjacency.FirstName, secondRoom) && Equals(adjacency.SecondName, firstRoom)))
            {
                return adjacency.Id;
            }
        }

        return 0;
    }

    private void ResetState()
    {
        _timer.Reset();
        _pathInformation.isDoorOpened = false;
    }

    private void ProcessChasing()
    {
        if (Equals(_pathInformation.playerLocation, _pathInformation.enemyLocation))
        {
            _femaleDummyMovement.SetTarget(_maleDummyMovement.transform);
        }
        else
        {
            _state = State.Processing_AI;
        }
    }

    private void ProcessExpectation()
    {
        if (_timer.IsOver)
        {
            _state = State.Door_Opening_By_Timer;
        }

        if (_pathInformation.isDoorOpened)
        {
            _state = State.Player_Opens_Door;
        }
    }
}
