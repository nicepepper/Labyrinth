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
    private MaleDummyMovement _maleDummyMovement;
    private DamageTaking _damageTaking;
    private FemaleDummyMovement _femaleDummyMovement;
    private CameraFollow _cameraFollow;
    private GameLogic _gameLogic;

    private void Awake()
    {
        _allUI = new [] {_inGameUI, _mainMenuUI, _pausedUI, _gameOverUI, _winningUI};
        
        StartGame();
        Time.timeScale = 0.0f;
    }

    private void Start()
    {
        ShowUI(_mainMenuUI);
    }

    public void StartGame()
    {
        ShowUI(_inGameUI);
        Time.timeScale = 1.0f;
        
        if (_currentFemaleDummy != null)
        {
            Destroy(_currentFemaleDummy);
        }

        if (_currentMaleDummy != null)
        {
            Destroy(_currentMaleDummy);
        }
        
        _currentFemaleDummy = Instantiate(
            _femaleDummyPrefab,
            _femaleDummyStartPoint.position,
            _femaleDummyStartPoint.rotation);

        _currentMaleDummy = Instantiate(
            _maleDummyPrefab,
            _maleDummyStartPoint.position,
            _maleDummyStartPoint.rotation);

        _maleDummyMovement = _currentMaleDummy.GetComponent<MaleDummyMovement>();
        _damageTaking = _currentMaleDummy.GetComponent<DamageTaking>();
        _femaleDummyMovement = _currentFemaleDummy.GetComponent<FemaleDummyMovement>();
        _cameraFollow = _camera.GetComponent<CameraFollow>();
        
        _damageTaking.SetGameManager(this);
        
        _cameraFollow.SetTarget(_maleDummyMovement.transform);
        _maleDummyMovement.SetCamera(_camera);
        _maleDummyMovement.Pause += SetPaused;
        
        _informationLogic.SetPalyer(_currentMaleDummy.transform);
        _informationLogic.SetEnemy(_currentFemaleDummy.transform);
            
        _gameLogic = gameObject.GetComponent<GameLogic>();
        _gameLogic.SetMaleDummyMovement(_maleDummyMovement);
        _gameLogic.SetFemaleDummyMovement(_femaleDummyMovement);
    }
    
    
    public void ShowUI(GameObject newUI)
    {
        foreach (var UiToHide in _allUI)
        {
            UiToHide.SetActive(false);
        }
        
        newUI.SetActive(true);
        Cursor.visible = !Equals(newUI, _inGameUI) ;
    }
    
    public void SetPaused(bool paused)
    {
        _inGameUI.SetActive(!paused);
        _pausedUI.SetActive(paused);
        
        Cursor.visible = paused;
        Time.timeScale = paused ? 0.0f : 1.0f;
    }
    
    public void GameOver()
    {
        ShowUI(_gameOverUI);
        _gameLogic.SetReset();
        Time.timeScale = 0.0f;
    }

    public void Winning()
    {
        ShowUI(_winningUI);
        _gameLogic.SetReset();
        Time.timeScale = 0.0f;
    }
}
