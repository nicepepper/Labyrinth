using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private float _timerStart = 30.0f;
    [SerializeField] private float _timerEnd = 0.00f;
    [SerializeField] private Text _timerText;
    
    private float _currentTime;
    private bool _timerRunning = true;
    private bool _isOver = false;
    
    private void Start()
    {
        _timerText.text = _timerStart.ToString("f2");
        _currentTime = _timerStart;
    }

    private void Update()
    {
        if (_timerRunning)
        {
            _currentTime -= Time.deltaTime;
            if (_currentTime <= _timerEnd)
            {
                _timerRunning = false;
                _isOver = true;
                _timerText.text = _timerEnd.ToString("f2");
            }
            _timerText.text = _currentTime.ToString("f2");
        }
    }

    private void Pause()
    {
        _timerRunning = false;
    }

    private void Restart()
    {
        _currentTime = _timerStart;
        _timerRunning = true;
        _isOver = false;
    }
}
