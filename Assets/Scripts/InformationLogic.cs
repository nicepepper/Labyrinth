using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InformationLogic : MonoBehaviour
{

    [SerializeField] private Transform _player;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _enemy;

    [SerializeField] private MapSettings _mapSettings;
    [SerializeField] private PathInformation _pathInformation;
    
    private Graph _graph = new Graph();
    private Dijkstra _dijkstra;
    
    private string _currentPlayerLoc = "5";
    private string _currentTargetLoc = "8";
    private string _currentEnemyLoc = "8";

    private void Awake()
    {
        SetVerticesOfGraph();
        SetVertexAdjacency();
    }

    private void SetVerticesOfGraph()
    {
        foreach (var vertex in _mapSettings.vertices)
        {
            _graph.AddVertex(vertex.Name);
            Debug.Log($"{vertex.Name}, {vertex.FirstPoint.X.ToString("f2")}, {vertex.FirstPoint.Z.ToString("f2")}");
        }
    }

    private void SetVertexAdjacency()
    {
        foreach (var adjacency in _mapSettings.adjacencies)
        {
            _graph.AddEdge(adjacency.FirstName, adjacency.SecondName, adjacency.EdgeWeight);
        }
    }

    private void Start()
    {
        _dijkstra = new Dijkstra(_graph);
        //InitLocationData();
    }

    private void InitLocationData()
    {
        СountNumberOfDoors( 
            _currentPlayerLoc, _pathInformation.PlayerLocation, 
            _currentTargetLoc, _pathInformation.TargetLocation,
            _pathInformation.NumberOfDoorsOnPath);
        
        СountNumberOfDoors( 
            _currentPlayerLoc, _pathInformation.PlayerLocation, 
            _currentEnemyLoc, _pathInformation.EnemyLocation, 
            _pathInformation.NumberOfDoorsOnPathToEnemy);
    }

    private void Update()
    {
        // ReplaceString(_currentPlayerLoc, _pathInformation.PlayerLocation);
        // ReplaceString(_currentTargetLoc, _pathInformation.TargetLocation);
        // ReplaceString(_currentEnemyLoc, _pathInformation.EnemyLocation);
        //
        // FindObjectLocation(_player, _pathInformation.PlayerLocation);
        // FindObjectLocation(_target, _pathInformation.TargetLocation);
        // FindObjectLocation(_enemy, _pathInformation.EnemyLocation);
        //
        // СountNumberOfDoors(
        //     _currentPlayerLoc, _pathInformation.PlayerLocation, 
        //     _currentTargetLoc, _pathInformation.TargetLocation, 
        //     _pathInformation.NumberOfDoorsOnPath);
        //
        // СountNumberOfDoors( 
        //     _currentPlayerLoc, _pathInformation.PlayerLocation, 
        //     _currentEnemyLoc, _pathInformation.EnemyLocation, 
        //     _pathInformation.NumberOfDoorsOnPathToEnemy);
        
        FindObjectLocation(_player, _pathInformation.PlayerLocation);
    }

    private void TestFunrion()
    {
        var text1 = "Good";
        var text2 = "Bad";
        string text3;
        text3 = text1;
        text1 = text2;
        text2 = "NOOOO";

        // if (Equals(text1, text2))
        // {
        //     Debug.Log("Yes");
        // }
        // else
        // {
        //     Debug.Log("No");
        // }
        
        Debug.Log(text1 + "," + text2 + "," + text3);
    }
    
    private void ReplaceString(string stringToCompare, string stringToReplacement)
    {
        if (Equals(stringToCompare, stringToReplacement))
        {
            return;
        }
        
        stringToCompare = stringToReplacement;
    }

    private void FindObjectLocation(Transform target, string strLoc)
    {
        
        foreach (var vertex in _mapSettings.vertices)
        {
            Debug.Log(target.position.x + "," + vertex.FirstPoint.X + "," + target.position.x + "," + vertex.SecondPoint.X);
            if (target.position.x >= vertex.FirstPoint.X && target.position.x < vertex.SecondPoint.X)
            {
                //Debug.Log(target.position.z + "," + vertex.FirstPoint.Z + "," + target.position.z + "," + vertex.SecondPoint.Z);
                if (target.position.z >= vertex.FirstPoint.Z && target.position.z < vertex.SecondPoint.Z)
                {
                    strLoc = vertex.Name;
                    Debug.Log(strLoc);
                    break;
                }
            }
        }
    }

    private void СountNumberOfDoors(string currentFirstLoc, string firstLoc, string currentSecondLoc, string secondLoc, int numberOfDoors )
    {
        var test = _pathInformation.NumberOfDoorsOnPathToEnemy;

        Debug.Log(test.ToString());
        Debug.Log(currentFirstLoc + "," + firstLoc + "," + currentSecondLoc + "," + secondLoc);

        if (!Equals(currentFirstLoc, firstLoc) || !Equals(currentSecondLoc, secondLoc))
        {
            FindNumberOfDoorsPath(firstLoc, secondLoc, numberOfDoors);
           
            Debug.Log(test.ToString());
        }
    }
    
    private void FindNumberOfDoorsPath(string startVertex, string finishVertex, int pathLength)
    {
        var path = _dijkstra.GetNumberVerticesInPath(startVertex, finishVertex);
        pathLength = path - 1;
    }
}
