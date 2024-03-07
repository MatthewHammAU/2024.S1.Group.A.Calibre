using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 360f; // Degrees per second

    private Vector2 moveInput;
    private Vector2 lookInput;

    private PlayerInput playerInput;

    private void Awake()
    {
        // Initialize DOTween (if not already done elsewhere in your project)
        DOTween.Init();

        // Get the PlayerInput component
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        // Subscribe to input events
        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Look"].performed += OnLook;
    }

    private void OnDisable()
    {
        // Unsubscribe from input events
        playerInput.actions["Move"].performed -= OnMove;
        playerInput.actions["Look"].performed -= OnLook;
    }

    void Update()
    {
        Move();
        Look();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    void Move()
    {
        Vector2 normalizedMoveInput = moveInput.normalized;
        Vector3 move = new Vector3(normalizedMoveInput.x, 0, normalizedMoveInput.y) * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);
    }

    void Look()
    {
        // Use the left stick input for looking/rotating
        Vector2 normalizedLookInput = lookInput.normalized;
        Vector3 direction = new Vector3(normalizedLookInput.x, 0, normalizedLookInput.y).normalized;

        if (direction != Vector3.zero)
        {
            // Calculate the target rotation
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            // Smoothly rotate using DOTween
            transform.DORotateQuaternion(targetRotation, 1 / rotationSpeed).SetEase(Ease.OutSine);
        }
    }

}
