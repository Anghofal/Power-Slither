using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraCenter : MonoBehaviour
{
    [Range(0.0f, 0.5f)]
    [SerializeField] private float cameraSmoothSpeed;
    [SerializeField] private Transform snakeHead;
    private Vector3 cameraVelocity;
    public bool EnableCamera;
    private void FixedUpdate()
    {
        if (EnableCamera)
        {
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(
                snakeHead.position.x, snakeHead.position.y, transform.position.z), ref cameraVelocity,
                cameraSmoothSpeed);
        }
    }
}
