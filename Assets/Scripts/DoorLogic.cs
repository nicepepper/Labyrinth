using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoorConnections
{
    public int nameDoor;
    public List<int> dependentDoors;
}

public class DoorLogic : MonoBehaviour
{
    [SerializeField] private MapSettings _mapSettings;
    [SerializeField] private Door[] _doors;
    private List<DoorConnections> _doorConnectionses = new List<DoorConnections>(); 

    private void Awake()
    {
        foreach (var door in _doors)
        {
            door.OnOpen += CloseConnectedDoors;
        }
        InitializeDoorsRelationship();

        if (_doorConnectionses.Count == 0)
        {
            Debug.Log("_doorConnectionses.Count == 0");
        }
        // foreach (var el in _doorConnectionses)
        // {
        //     Debug.Log("nameDoor" + el.nameDoor);
        //     foreach (var doorD in el.dependentDoors)
        //     {
        //         Debug.Log(doorD + ",");
        //     }
        // }
    }

    private void Update()
    {
        
    }

    private void CloseConnectedDoors(int idOpenDoor)
    {
        
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

        FindAdjacentVerticesById(ref vertexNames, idOpenDoor);
        FindIdDoorsToClose(ref idDoorsToClose, vertexNames);
        RemoveUnnecessaryDoorID(ref idDoorsToClose, idOpenDoor);

        el.nameDoor = idOpenDoor;
        foreach (var id in idDoorsToClose)
        {
            el.dependentDoors.Add(id);
        }

        return el;
    }

    private void FindAdjacentVerticesById(ref List<string> list, int id)
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
    
    private void FindIdDoorsToClose(ref  List<int> idList, List<string> names)
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

    private void RemoveUnnecessaryDoorID(ref List<int> list, int value)
    {
        IEnumerable<int> distinctId = list.Distinct();
        list.Clear();
        
        foreach (var id in distinctId)
        {
            list.Add(id);
        }

        list.Remove(value);
    }
}
