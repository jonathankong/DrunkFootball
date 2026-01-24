using PurrNet;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class TestPlayerController : MonoBehaviour
{
    private InputActions _inputActions;

    [SerializeField] private List<IControllable> _playerObjs = new();
    [SerializeField] private IControllable _currentObj;
    [SerializeField] private IControllable _nextObj;
    [SerializeField] private int _currentObjIndex;
    [SerializeField] private int _initialObjIndex = 0;
    [SerializeField] private GameObject _startPanel;

    private void Awake()
    {
        _inputActions = new InputActions();
        GetComponentsInChildren(true, _playerObjs);
        _currentObj = _playerObjs[_initialObjIndex];
        _currentObjIndex = _initialObjIndex;
        _nextObj = _playerObjs[(_currentObjIndex + 1) % _playerObjs.Count];
    }

    private void Jump_performed(InputAction.CallbackContext obj)
    {
        _currentObj = _nextObj;
        _currentObjIndex += 1;
        _nextObj = _playerObjs[(_currentObjIndex + 1) % _playerObjs.Count];

        Debug.Log($"Swap performed! Current object is now {((MonoBehaviour)_currentObj).gameObject.name} and the next object is {((MonoBehaviour)_currentObj).gameObject.name}");
    }

    private void Attack_performed(InputAction.CallbackContext obj)
    {
        _currentObj.AttackPressed();
    }

    private void OnEnable()
    {
        _inputActions.UI.Enable();
        _inputActions.Player.Attack.performed += Attack_performed;
        _inputActions.Player.Jump.performed += Jump_performed;
        _inputActions.UI.Submit.performed += Submit_performed;
    }

    private void Submit_performed(InputAction.CallbackContext obj)
    {
        _startPanel.SetActive(false);
        _inputActions.Player.Enable();
        _inputActions.UI.Disable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
        _inputActions.Player.Attack.performed -= Attack_performed;
        _inputActions.Player.Jump.performed -= Jump_performed;
        _inputActions.UI.Submit.performed -= Submit_performed;
    }
}
