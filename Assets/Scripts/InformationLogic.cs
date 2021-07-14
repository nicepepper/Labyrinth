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
    
    private string _currentPlayerLoc;
    private string _currentTargetLoc;
    private string _currentEnemyLoc;

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
        InitLocationData();
    }

    private void InitLocationData()
    {
        FindObjectLocation(_player, ref _currentPlayerLoc);
        FindObjectLocation(_target, ref _currentTargetLoc);
        FindObjectLocation(_enemy, ref _currentEnemyLoc);
        
        _pathInformation.playerLocation = _currentPlayerLoc;
        _pathInformation.targetLocation = _currentTargetLoc;
        _pathInformation.enemyLocation = _currentEnemyLoc;

        _pathInformation.numberOfDoorsOnPath = _dijkstra.GetNumberVerticesInPath(_pathInformation.playerLocation, _pathInformation.targetLocation) - 1;
        _pathInformation.numberOfDoorsOnPathToEnemy = _dijkstra.GetNumberVerticesInPath(_pathInformation.playerLocation, _pathInformation.enemyLocation) - 1;
    }

    private void Update()
    {
        FindObjectLocation(_player, ref _currentPlayerLoc);
        FindObjectLocation(_target, ref _currentTargetLoc);
        FindObjectLocation(_enemy, ref _currentEnemyLoc);
        
        FindNumberOfDoorsOnPath(_currentPlayerLoc, _pathInformation.playerLocation, _currentTargetLoc, _pathInformation.targetLocation, ref _pathInformation.numberOfDoorsOnPath);
        FindNumberOfDoorsOnPath(_currentPlayerLoc, _pathInformation.playerLocation, _currentEnemyLoc, _pathInformation.enemyLocation, ref _pathInformation.numberOfDoorsOnPathToEnemy);
        
        Replace<string>(ref _pathInformation.playerLocation, ref _currentPlayerLoc);
        Replace<string>(ref _pathInformation.targetLocation, ref _currentTargetLoc);
        Replace<string>(ref _pathInformation.enemyLocation, ref _currentEnemyLoc);
    }
    
    public static void Replace<T> (ref T oldValue, ref T newValue)
    {
        if (Equals(oldValue, newValue))
        {
            return;
        }

        oldValue = newValue;
    }
    
    private void FindObjectLocation(Transform target, ref string strLoc)
    {
        foreach (var vertex in _mapSettings.vertices)
        {
            if (target.position.x >= vertex.FirstPoint.X && target.position.x < vertex.SecondPoint.X)
            {
                if (target.position.z >= vertex.FirstPoint.Z && target.position.z < vertex.SecondPoint.Z)
                {
                    strLoc = vertex.Name;
                    break;
                }
            }
        }
    }
    
    private void FindNumberOfDoorsOnPath(string currentFirstLoc, string firstLoc, string currentSecondLoc, string secondLoc, ref int numberOfDoors)
    {
        if (!Equals(currentFirstLoc, firstLoc) || !Equals(currentSecondLoc, secondLoc))
        {
            var path = _dijkstra.GetNumberVerticesInPath(currentFirstLoc, currentSecondLoc);
            numberOfDoors = path - 1;
        }
    }

    public void SetPalyer(Transform _transform)
    {
        _player = _transform;
    }
    
    public void SetEnemy(Transform _transform)
    {
        _enemy = _transform;
    }
}
