using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject _maleDummyPrefab;
    [SerializeField] private Transform _maleDummyStartPoint;
    [SerializeField] private GameObject _currentMaleDummy;
    [Space]
    [SerializeField] private GameObject _femaleDummyPrefab;
    [SerializeField] private Transform _femaleDummyStartPoint;
    [SerializeField] private GameObject _currentFemaleDummy;
    [Space]
    [SerializeField] private GameObject _inGameUI;
    [SerializeField] private GameObject _mainMenuUI;
    [SerializeField] private GameObject _pausedUI;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private GameObject _winningUI;
    [Space]
    [SerializeField] private Transform _camera;
    [SerializeField] private InformationLogic _informationLogic;

    private GameObject[] _allUI;
    private bool paused;

    private void Awake()
    {
        _allUI = new [] {_inGameUI, _mainMenuUI, _pausedUI, _gameOverUI, _winningUI};
        
        _currentFemaleDummy = Instantiate(
            _femaleDummyPrefab,
            _femaleDummyStartPoint.position,
            _femaleDummyStartPoint.rotation);

        _currentMaleDummy = Instantiate(
            _maleDummyPrefab,
            _maleDummyStartPoint.position,
            _maleDummyStartPoint.rotation);

        var maleDummyMovement = _currentMaleDummy.GetComponent<MaleDummyMovement>();
        var femaleDummyMovement = _currentFemaleDummy.GetComponent<FemaleDummyMovement>();
        var cameraFollow = _camera.GetComponent<CameraFollow>();
        
        cameraFollow.SetTarget(maleDummyMovement.transform);
        maleDummyMovement.SetCamera(_camera);
        maleDummyMovement.Pause += SetPaused;
        
        _informationLogic.SetPalyer(_currentMaleDummy.transform);
        _informationLogic.SetEnemy(_currentFemaleDummy.transform);

        var logic = gameObject.GetComponent<GameLogic>();
        logic.SetMaleDummyMovement(maleDummyMovement);
        logic.SetFemaleDummyMovement(femaleDummyMovement);
    }

    private void Start()
    {
        
        
        ShowUI(_mainMenuUI);
    }

    public void StartGame()
    {
        ShowUI(_inGameUI);
    }
    
    
    private void ShowUI(GameObject newUI)
    {
        foreach (var UiToHide in _allUI)
        {
            UiToHide.SetActive(false);
        }
        
        newUI.SetActive(true);
    }
    
    public void SetPaused(bool paused)
    {
        _inGameUI.SetActive(!paused);
        _pausedUI.SetActive(paused);

        if (paused)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }
    
    public void GameOver()
    {
        ShowUI(_gameOverUI);
    }
}
