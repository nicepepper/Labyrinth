using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PathInformation", menuName = "Labyrinth/PathInformation", order = 3)]
public class PathInformation : ScriptableObject
{
    [SerializeField] public string playerLocation;
    [SerializeField] public string targetLocation;
    [SerializeField] public string enemyLocation;
    
    [SerializeField] public int numberOfDoorsOnPath;
    [SerializeField] public int numberOfDoorsOnPathToEnemy;
    [SerializeField] public int counterOpenDoors;

    [SerializeField] public List<int> lockableDoors = new List<int>();

    // public string PlayerLocation
    // {
    //     get => _playerLocation;
    //     set => _playerLocation = value;
    // }
    //
    // public string TargetLocation
    // {
    //     get => _targetLocation;
    //     set => _targetLocation = value;
    // }
    //
    // public string EnemyLocation
    // {
    //     get => _enemyLocation;
    //     set => _enemyLocation = value;
    // }
    //
    // public int NumberOfDoorsOnPath
    // {
    //     get => _numberOfDoorsOnPath;
    //     set => _numberOfDoorsOnPath = value;
    // }
    //
    // public int NumberOfDoorsOnPathToEnemy
    // {
    //     get => _numberOfDoorsOnPathToEnemy;
    //     set => _numberOfDoorsOnPathToEnemy = value;
    // }
}
