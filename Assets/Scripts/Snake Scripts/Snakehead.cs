using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snakehead : MonoBehaviour
{
    
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float snakeMovSpeed;
    [SerializeField] private float snakeRotationSensivity;
    private Vector3 snakeLastMoveDir = new Vector3(0,1,0);

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Get Vector2 from GameInput Class
        Vector2 snakeVector2 = gameInput.GetMovementVector2Normalized();
        
        // Asign the vector from var snakeVector2 into snakeMoveDirection
        Vector3 snakeMoveDir = new Vector3(snakeVector2.x, snakeVector2.y, 0);

        // Make new var Distance later we will multiply with snakeMoveDir
        // and to know how far snake will go over seconds using movement speed parameter
        float snakeMoveDistance = snakeMovSpeed * Time.deltaTime;

        // If player is not input anything snake will still walk
        // using last input player
        if (snakeMoveDir != Vector3.zero)
        {
            snakeLastMoveDir = snakeMoveDir;
        }

        // Making rotation based on move direction
        Quaternion snakeDir = Quaternion.LookRotation(Vector3.forward, snakeLastMoveDir);

        // Move position head based on move direction (input) and distance (speed) 
        transform.position += snakeLastMoveDir * snakeMoveDistance;

        // Make head rotation to
        transform.rotation = Quaternion.Lerp(transform.rotation, snakeDir, snakeRotationSensivity * Time.deltaTime);
    }
}
