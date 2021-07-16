using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILogic : MonoBehaviour
{
    [SerializeField] private FemaleDummyMovement _femaleDummyMovement;
    [SerializeField] private MaleDummyMovement _maleDummyMovement;
    [SerializeField] private DoorLogic _doorLogic;
    [SerializeField] private MapSettings _mapSettings;
    [SerializeField] private PathInformation _pathInformation;

    private bool _isPlayer;
    private bool _isToPlayer;
    private bool _isCenter;
    private bool _isToCenter;
    private bool _isNewOpenDoor;
    private bool _isToNewOpedDoor;
    private bool _isRoutine;

    private int _previousOpenDoorId;
    private int _nextOpenDoorId;
    private string _previousRoom;
    private string _nextRoom;

    private GameObject _previosCenterRoom;
    private GameObject _nextCenterRoom;


    private void Awake()
    {
        _previosCenterRoom = new GameObject();
        _nextCenterRoom = new GameObject();
    }

    public void StartRoutine()
    {
        if (_femaleDummyMovement != null && _maleDummyMovement != null)
        {
            _isRoutine = true;
            StartCoroutine(RouteAIRoutine());
        }
        else
        {
            Debug.Log("dont targets!!!");
        }
    }

    public void StopRoutine()
    {
        _isRoutine = false;
    }

    public void Init()
    {
        _isPlayer = false;
        _isCenter = false;
        _isNewOpenDoor = false;
        _previousOpenDoorId = 0;
        _nextOpenDoorId = 0;
    }
    
    public void SetFemaleDummyMovement(FemaleDummyMovement femaleDummyMovement)
    {
        _femaleDummyMovement = femaleDummyMovement;
    }

    public void SetMaleDummyMovement(MaleDummyMovement maleDummyMovement)
    {
        _maleDummyMovement = maleDummyMovement;
    }

    public void SetPathInformation(PathInformation pathInformation)
    {
        _pathInformation = pathInformation;
    }

    public void SetNextOpenDoorId(int doorId)
    {
        _previousOpenDoorId = _nextOpenDoorId; 
        _nextOpenDoorId = doorId;
        _previousRoom = _pathInformation.enemyLocation;
        _nextRoom = GetNameOfNextLocation(_nextOpenDoorId, _previousRoom);
        
        _previosCenterRoom = _nextCenterRoom;
        _nextCenterRoom.transform.position = GetCenterOfRoom(_nextRoom);
        _isNewOpenDoor = true;
    }

    private IEnumerator RouteAIRoutine()
    {
        Debug.Log("startCorutine");
        while (_isRoutine)
        {
            if (!_femaleDummyMovement.IsTarget())
            {
                _isToPlayer = false;
                _isToNewOpedDoor = false;
            }

            if (Equals(_nextRoom, _pathInformation.enemyLocation))
            {
                _isNewOpenDoor = false;
            }

            if (Equals(_pathInformation.enemyLocation, _pathInformation.playerLocation))
            {
                if (!_isToPlayer)
                {
                    _femaleDummyMovement.SetTarget(_maleDummyMovement.transform);
                    _isToPlayer = true;
                    _isToNewOpedDoor = false;
                    _isToCenter = false;
                }
                
                continue;
            }
            
            if (_isNewOpenDoor)
            {
                if (!_isToNewOpedDoor)
                {
                    _femaleDummyMovement.SetTarget(_doorLogic.GetTransformDoor(_nextOpenDoorId));
                    _isToPlayer = false;
                    _isToNewOpedDoor = true;
                    _isToCenter = false;
                }
                
                continue;
            }
            
            if (!_isToCenter)
            {
                _femaleDummyMovement.SetTarget(_nextCenterRoom.transform);
                _isToPlayer = false;
                _isToNewOpedDoor = false;
                _isToCenter = true;
            }

            yield return null;
        }
        Debug.Log("stopCorutine");
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
}
