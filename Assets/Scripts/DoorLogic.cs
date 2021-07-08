using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLogic : MonoBehaviour
{
    [SerializeField] private MapSettings _mapSettings;
    [SerializeField] private Door[] _doors;
    
    private void Update()
    {
        
    }

    private void CloseConnectedDoors(int idOpenDoor)
    {
        List<int> idDoorsToClose = new List<int>();
        List<string> vertexNames = new List<string>();
        
        FindAdjacentVerticesById(ref vertexNames, idOpenDoor);
        
        FindDependentDoors(ref idDoorsToClose, vertexNames);
        
        
    }

    private void FindDependentDoors(ref  List<int> idList, List<string> list)
    {
        foreach (var el in list)
        {
            foreach (var adjacency in _mapSettings.adjacencies)
            {
                if (adjacency.FirstName == el || adjacency.SecondName == el)
                {
                    idList.Add(adjacency.Id);
                }
            }
        }
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
}
