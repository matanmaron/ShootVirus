using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float Speed = 15;
    [SerializeField] float sensitivity = 10;
    [SerializeField] bool invert = false;
    [SerializeField] bool lockCamera = false;
    [SerializeField] GameObject CameraHolder;
    float player_rotationX;
    float camera_minimumY = -10;
    float camera_maximumY = 30;
    float camera_rotationY;
    float camera_rotationX;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        MovePlayer();
        MoveCamera();
    }

    void MovePlayer()
    {
        var inputY = Input.GetAxis("Vertical");
        var inputX = Input.GetAxis("Horizontal");
        transform.position += transform.forward * Speed * inputY * Time.deltaTime;
        player_rotationX += inputX * sensitivity;
        transform.localEulerAngles = new Vector3(0, player_rotationX, 0);
        if (lockCamera)
        {
            camera_rotationX += inputX * sensitivity;
            CameraHolder.transform.localEulerAngles = new Vector3(0, camera_rotationX, 0);
            CameraHolder.transform.position = transform.position;
        }
    }

    void MoveCamera()
    {
        if (lockCamera)
        {
            return;
        }
        if (invert)
        {
            camera_rotationY -= Input.GetAxis("Mouse Y") * sensitivity;
        }
        else
        {
            camera_rotationY += Input.GetAxis("Mouse Y") * sensitivity;    
        }
        camera_rotationX += Input.GetAxis("Mouse X") * sensitivity;
        camera_rotationY = Mathf.Clamp(camera_rotationY, camera_minimumY, camera_maximumY);
        CameraHolder.transform.localEulerAngles = new Vector3(-camera_rotationY, camera_rotationX, 0);
        CameraHolder.transform.position = transform.position;
    }
}
