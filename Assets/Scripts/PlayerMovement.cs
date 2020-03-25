using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2f;
    private CharacterController _controller;
    private Animator _animator;
    private Transform _mainCamera;
    private float currentSpeed = 0f;
    private float speedSmoothVelocity = 0f;
    private float speedSmoothTime = 0.1f;
    private float rotationSpeed = 0.1f;
    private float gravity = 3f;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        if (_controller == null)
        {
            Debug.LogError("PlayerMovement, Start, controller not found");
            Application.Quit();;
        }
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("PlayerMovement, Start, animator not found");
            Application.Quit();;
        }

        var cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("PlayerMovement, Start, camera not found");
            Application.Quit();
        }
        // ReSharper disable once PossibleNullReferenceException
        _mainCamera = cam.transform;
        // ReSharper disable once InvertIf
        if (_mainCamera == null)
        {
            Debug.LogError("PlayerMovement, Start, camera transform not found");
            Application.Quit();;
        }
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 forward = _mainCamera.forward;
        Vector3 right = _mainCamera.right;

        forward.y = 0;
        right.y = 0;
        
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;
        Vector3 gravityVector = Vector3.zero;

        if (!_controller.isGrounded)
        {
            gravityVector.y -= gravity;
        }

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection),rotationSpeed );
        }
        float targetSpeed = movementSpeed * moveInput.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        _controller.Move(moveDirection * (currentSpeed * Time.deltaTime));
        _controller.Move(gravityVector * Time.deltaTime);
    }
}
