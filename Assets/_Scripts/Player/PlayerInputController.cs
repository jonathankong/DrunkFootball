using PurrNet;
using System;
using System.Data;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : NetworkIdentity
{
    [Header("Input")]
    private LocalInputReader _playerInputReader;

    
    private void Awake()
    {
        // Grab components on the same GameObject if not wired in Inspector
        if (_playerInputReader == null)
            _playerInputReader = GetComponent<LocalInputReader>();
    }

    public void Confirm()
    {

    }
}
