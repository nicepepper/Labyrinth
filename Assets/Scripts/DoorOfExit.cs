using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class DoorOfExit : MonoBehaviour, IInteractable
{
    [SerializeField] private GameManager _game;

    private bool _isOpened;

    private void Awake()
    {
        _isOpened = false;
    }

    public void Action()
    {
        Debug.Log("Winning!");
        _game.Winning();
        _isOpened = true;
    }

    public bool IsActed()
    {
        return _isOpened;
    }
}
