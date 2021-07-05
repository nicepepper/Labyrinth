using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PathInformation", menuName = "Labyrinth/PathInformation", order = 3)]
public class PathInformation : ScriptableObject
{
    [SerializeField] private string _playerLocation;
    [SerializeField] private string _targetLocation;
    [SerializeField] private string _enemyLocation;
    
    [SerializeField] private int _numberOfDoorsOnPath;
    [SerializeField] private int _numberOfDoorsOnPathToEnemy;

    public string PlayerLocation
    {
        get => _playerLocation;
        set => _playerLocation = value;
    }

    public string TargetLocation
    {
        get => _targetLocation;
        set => _targetLocation = value;
    }

    public string EnemyLocation
    {
        get => _enemyLocation;
        set => _enemyLocation = value;
    }

    public int NumberOfDoorsOnPath
    {
        get => _numberOfDoorsOnPath;
        set => _numberOfDoorsOnPath = value;
    }

    public int NumberOfDoorsOnPathToEnemy
    {
        get => _numberOfDoorsOnPathToEnemy;
        set => _numberOfDoorsOnPathToEnemy = value;
    }
}
