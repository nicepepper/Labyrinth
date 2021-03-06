using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoorConnections
{
    public int idDoor = 0;
    public List<int> dependentDoors = new List<int>();
}

public class DoorLogic : MonoBehaviour
{
    [SerializeField] private MapSettings _mapSettings;
    [SerializeField] private Door[] _doors;
    [SerializeField] private PathInformation _pathInformation;
    
    private List<DoorConnections> _doorConnectionses = new List<DoorConnections>();
    
    private void Awake()
    {
        foreach (var door in _doors)
        {
            door.OnOpen += CloseConnectedDoors;
        }
        
        InitializeDoorsRelationship();
    }

    public void OpenDoorById(int id)
    {
        foreach (var door in _doors)
        {
            if (door.IdDoor == id)
            {
                door.Open();
            }
        }
    }

    public bool IsActedDoorById(int id)
    {
        foreach (var door in _doors)
        {
            if (door.IdDoor == id)
            {
                return door.IsActed();
            }
        }
        
        return false;
    }

    public Transform GetTransformDoor(int doorId)
    {
        foreach (var door in _doors)
        {
            if (door.IdDoor == doorId)
            {
                return door.Transform;
            }
        }

        return null;
    }

    public void CloseAllDoors()
    {
        foreach (var door in _doors)
        {
            door.Close();
        }
    }

    private void CloseConnectedDoors(int idOpenDoor)
    {
        foreach(var connectionse in _doorConnectionses)
        {
            if (connectionse.idDoor == idOpenDoor)
            {
                _pathInformation.lockableDoors.Clear();
                foreach (var id in connectionse.dependentDoors)
                {
                    _pathInformation.lockableDoors.Add(id);
                    CloseDoor(id);
                }
            }
        }
    }

    private void CloseDoor(int id)
    {
        foreach (var door in _doors)
        {
            if (door.IdDoor == id)
            {
                door.Close();
                _pathInformation.lockableDoors.Add(door.IdDoor);
            }
        }
    }
    
    private void InitializeDoorsRelationship()
    {
        _doorConnectionses.Capacity = _mapSettings.adjacencies.Count;
       
        foreach (var adjacency in _mapSettings.adjacencies)
        {
            _doorConnectionses.Add(FindDoorConnection(adjacency.Id));
        }
    }
    
    private DoorConnections FindDoorConnection(int idOpenDoor)
    {
        List<int> idDoorsToClose = new List<int>();
        List<string> vertexNames = new List<string>();
        DoorConnections el = new DoorConnections();

        FindAdjacentVerticesById(vertexNames, idOpenDoor);
        FindIdDoorsToClose(idDoorsToClose, vertexNames);
        RemoveUnnecessaryDoorID(idDoorsToClose, idOpenDoor);

        el.idDoor = idOpenDoor;
        foreach (var id in idDoorsToClose)
        {
            el.dependentDoors.Add(id);
        }
        
        return el;
    }

    private void FindAdjacentVerticesById(List<string> list, int id)
    {
        foreach (var adjacency in _mapSettings.adjacencies)
        {
            if (adjacency.Id == id)
            {
                list.Add(adjacency.FirstName);
                list.Add(adjacency.SecondName);
                break;
            }
        }
    }
    
    private void FindIdDoorsToClose(List<int> idList, List<string> names)
    {
        foreach (var name in names)
        {
            foreach (var adjacency in _mapSettings.adjacencies)
            {
                if (adjacency.FirstName == name || adjacency.SecondName == name)
                {
                    idList.Add(adjacency.Id);
                }
            }
        }
    }

    private void RemoveUnnecessaryDoorID(List<int> list, int value)
    {
        IEnumerable<int> distinctId =  new List<int>(list.Distinct());
        list.Clear();
        
        foreach (var id in distinctId)
        {
            list.Add(id);
        }

        list.Remove(value);
    }
}
