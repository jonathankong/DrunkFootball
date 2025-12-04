using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateController : MonoBehaviour
{
    [Header("References")]
    private IHasHealth _healthComp;
    private PlayerInput _playerInput;
    [SerializeField] private GameObject _iceBlock;

    [SerializeField] private float _stunTimeMS;
    [SerializeField] private bool _isStunned = false;

    private void Awake()
    {
        _healthComp = GetComponent<IHasHealth>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnDamage()
    {
        //Stun player
        Debug.Log("Player got hittttttt!");
        if (!_isStunned)
        {
            _isStunned = true;
            StartCoroutine(StunRoutine(_stunTimeMS));
        }
    }

    private IEnumerator StunRoutine(float _stunTimeMS)
    {
        Debug.Log($"Player stunned for {_stunTimeMS} milliseconds.");

        // Disable movement or controls here
        // For example, you can set a flag in your movement controller.
        _playerInput.DeactivateInput();


        yield return new WaitForSeconds(_stunTimeMS / 1000);
        _playerInput.ActivateInput();

        // Re-enable movement or controls
        Debug.Log("Player is no longer stunned.");
        _isStunned = false;
    }

    private IEnumerator FrozenRoutine(float _stunTimeMS)
    {
        Debug.Log($"Player stunned for {_stunTimeMS} milliseconds.");

        // Disable movement or controls here
        // For example, you can set a flag in your movement controller.
        _playerInput.DeactivateInput();


        yield return new WaitForSeconds(_stunTimeMS / 1000);
        _playerInput.ActivateInput();

        // Re-enable movement or controls
        Debug.Log("Player is no longer stunned.");
        _iceBlock.SetActive(false);
        _healthComp.FullHeal();
    }

    private void OnEnable()
    {
        _healthComp.Damaged += OnDamage;
        _healthComp.PlayerDied += OnDeath;
    }

    private void OnDeath()
    {
        if (_iceBlock == null) 
        {
            Debug.Log("No iceblock attached");
            return; 
        }
        Debug.Log($"Player Died");
        _iceBlock.SetActive(true);
        StartCoroutine(FrozenRoutine(_stunTimeMS * 3));
    }

    private void OnDisable()
    {
        _healthComp.Damaged -= OnDamage;
        _healthComp.PlayerDied -= OnDeath;
    }
}
