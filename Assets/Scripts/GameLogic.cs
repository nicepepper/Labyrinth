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
    Expectation,
    ProcessingAi,
    DoorOpeningByTimer,
    PlayerOpensDoor,
    Chasing,
    ResetState
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
    private bool _isMove;
    private bool _isReset;

    private void Start()
    {
        InitializeRoomsWithTheirDoors();
        ResetState();

        _isReset = false;
        _isMove = false;
    }

    private void Update()
    {
        if (_isReset)
        {
            _state = State.ResetState;
            Debug.Log(_state);
        }
        
        switch (_state)
        {
            case State.Begin:
                ProcessStart();
                break;
            
            case State.PlayerOpensDoor:
                ProcessPlayer();
                break;
            
            case State.ProcessingAi:
                ProcessAI();
                break;
            
            case State.DoorOpeningByTimer:
                ProcessOpenDoorOnTimer();
                break;

            case State.Expectation:
                ProcessExpectation();
                break;
            
            case State.ResetState:
                ResetState();
                break;
            
            case State.Chasing:
                ProcessChasing();
                break;
        }
    }

    public void SetFemaleDummyMovement(FemaleDummyMovement femaleDummyMovement)
    {
        _femaleDummyMovement = femaleDummyMovement;
    }
    
    public void SetMaleDummyMovement(MaleDummyMovement maleDummyMovement)
    {
        _maleDummyMovement = maleDummyMovement;
    }

    public void SetReset()
    {
        _isReset = true;
    }
    
    private void ProcessStart()
    {
        if (_pathInformation.isDoorOpened)
        {
            _state = State.PlayerOpensDoor;
            Debug.Log(_state);

        }
    }

    private void ProcessAI()
    {
        _state = State.Expectation;
        Debug.Log(_state);

        if (!_isMove && !_femaleDummyMovement.IsTarget())
        { 
            StartCoroutine(RouteAIRoutine());
        }
    }

    private void ProcessPlayer()
    {
        _pathInformation.isDoorOpened = false;
        _timer.Restart();
        _state = State.Chasing;
        Debug.Log(_state);
    }

    private void ProcessOpenDoorOnTimer()
    {
        _timer.Restart();
        
        if (_pathInformation.numberOfDoorsOnPathToEnemy == 1)
        {
            _state = State.ProcessingAi;
            Debug.Log(_state);
            return;
        }
        
        OpenNewRandomRoomDoor(_pathInformation.playerLocation);

        _state = State.Chasing;
        Debug.Log(_state);
    }

    private void ProcessChasing()
    {
        _state = State.ProcessingAi;
        Debug.Log(_state);
    }

    private void ProcessExpectation()
    {
        if (Equals(_pathInformation.playerLocation, _pathInformation.enemyLocation))
        {
            StopCoroutine(RouteAIRoutine());
            _isMove = false;
            _femaleDummyMovement.SetTarget(_maleDummyMovement.transform);
        }
        
        if (_pathInformation.isDoorOpened)
        {
            _state = State.PlayerOpensDoor;
            Debug.Log(_state);
        }
        
        if (_timer.IsOver)
        {
            _state = State.DoorOpeningByTimer;
            Debug.Log(_state);
        }
    }
    
    private void ResetState()
    {
        _isReset = false;
        _timer.Reset();
        _doorLogic.CloseAllDoors();
        _pathInformation.isDoorOpened = false;
        _state = State.Begin;
        Debug.Log(_state);
    }
    
    //костыль
    IEnumerator RouteAIRoutine()
    {
        _isMove = true;
        Stack<Transform> route = new Stack<Transform>(); 

        if (_pathInformation.numberOfDoorsOnPathToEnemy == 1)
        {
            _doorLogic.OpenDoorById(GetDoorId(_pathInformation.playerLocation, _pathInformation.enemyLocation));
        }
        else
        {
            OpenNewRandomRoomDoorExcludingLocked(_pathInformation.enemyLocation);
        }

        int idOpenDoor = FindOpenDoorOnRoom(_pathInformation.enemyLocation);
       
        GameObject center = new GameObject();
        var nextLoc = GetNameOfNextLocation(idOpenDoor, _pathInformation.enemyLocation);
        center.name = "center" + nextLoc;
        center.transform.position =  GetCenterOfRoom(nextLoc);

        route.Push(center.transform);
        route.Push(_doorLogic.GetTransformDoor(idOpenDoor));
        
        while ((route.Count != 0 || _femaleDummyMovement.IsTarget()) && !_isReset )
        {
            if (!_femaleDummyMovement.IsTarget() && route.Count != 0)
            {
                _femaleDummyMovement.SetTarget(route.Pop());
            }
            yield return null;
        }

        _isMove = false;
        route.Clear();
        Destroy(center);
    }
    
    private bool IsBadId(int id)
    {
        foreach (var door in _pathInformation.lockableDoors)
        {
            if (Equals(door, id))
            {
                return true;
            }
        }

        return false;
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

    private void OpenNewRandomRoomDoor(string roomName)
    {
        var rand = new Random();
        foreach (var roomWithDoors in _roomsWithDoors)
        {
            if (Equals(roomWithDoors.roomName, roomName))
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
                
                break;
            }
        }
    }

    private void OpenNewRandomRoomDoorExcludingLocked(string nameRoom)
    {
        var rand = new Random();
        foreach (var roomWithDoors in _roomsWithDoors)
        {
            if (Equals(roomWithDoors.roomName, nameRoom))
            {
                for (;;)
                {
                    var id = roomWithDoors.doorsId[rand.Next(0, roomWithDoors.doorsId.Count)];
                    
                    if (!_doorLogic.IsActedDoorById(id) && !IsBadId(id))
                    {
                        _doorLogic.OpenDoorById(id);
                        break;
                    }
                }
                
                break;
            }
        }
    }
    
    private int FindOpenDoorOnRoom(string roomName)
    {
        foreach (var roomWithDoors in _roomsWithDoors)
        {
            if (Equals(roomName, roomWithDoors.roomName))
            {
                foreach (var doorId in roomWithDoors.doorsId)
                {
                    if (_doorLogic.IsActedDoorById(doorId))
                    {
                        return doorId;
                    }
                }
            }
        }

        return 0;
    }

    private Vector3 GetCenterOfRoom(string roomName)
    {
        foreach (var vertex in _mapSettings.vertices)
        {
            if (Equals(vertex.Name, roomName))
            {
                Vector3 center = Vector3.zero;
                center.x = (vertex.FirstPoint.X + vertex.SecondPoint.X) / 2;
                center.z = (vertex.FirstPoint.Z + vertex.SecondPoint.Z) / 2;

                return center;
            }
        }

        return Vector3.zero;
    }

    private string GetNameOfNextLocation(int doorId, string currentLoc)
    {
        foreach (var adjacency in _mapSettings.adjacencies)
        {
            if (adjacency.Id == doorId)
            {
                return Equals(adjacency.FirstName, currentLoc) ? adjacency.SecondName : adjacency.FirstName;
            }
        }

        return null;
    }
}
