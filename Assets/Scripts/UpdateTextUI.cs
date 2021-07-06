using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTextUI : MonoBehaviour
{
    [SerializeField] private Text _openDoorCounter;
    [SerializeField] private Text _informationFieldPath;
    [SerializeField] private Text _informationFieldPathAI;
    [SerializeField] private PathInformation _pathInformation;

    // private void Start()
    // {
    //     if (_openDoorCounter == null || _informationFieldPath == null || _informationFieldPathAI || _pathInformation == null)
    //     {
    //         Debug.LogError("Object not specified!");
    //     }
    // }

    private void Update()
    {
        _openDoorCounter.text = _pathInformation.counterOpenDoors.ToString();
        _informationFieldPath.text = _pathInformation.numberOfDoorsOnPath.ToString();
        _informationFieldPathAI.text = _pathInformation.numberOfDoorsOnPathToEnemy.ToString();
    }
}
