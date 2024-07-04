using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private SnakeInputActions inputActions;

    private void Awake()
    {
        // Initialize the input system
        inputActions = new SnakeInputActions();
        inputActions.Snake.Enable();
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public Vector2 GetMovementVector2Normalized()
    {
        // Getting input from input system and assign into variable snakeVector
        Vector2 snakeVector = inputActions.Snake.Move.ReadValue<Vector2>();

        // normalized the input
        snakeVector = snakeVector.normalized;

        return snakeVector;
    }
}
