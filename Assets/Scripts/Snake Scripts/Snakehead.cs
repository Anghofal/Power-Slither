using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Snakehead : MonoBehaviour
{
    // var for movement head
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float snakeMovSpeed;
    [SerializeField] private float snakeRotationSensivity;
    private Vector3 snakeLastMoveDir = new Vector3(0,1,0);

    // var for body movement
    [SerializeField] private GameObject snakeBodyPrefab;
    private List<SnakeBody> listBodyParts = new List<SnakeBody>();
    [SerializeField] private float bodyFollowDistance;

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
        HeadMovement();
        BodyMovement();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddBodyParts();
        }
    }
    
    private void BodyMovement()
    {
        for (int i = 0; i < listBodyParts.Count; i++)
        {
            if (i != 0)
            {
                listBodyParts[i].transform.position = Vector3.Lerp(listBodyParts[i].transform.position,
                    listBodyParts[i - 1].transform.position, bodyFollowDistance);
                listBodyParts[i].transform.up = listBodyParts[i].transform.position - listBodyParts[i - 1].transform.position;
            }
            else
            {
                listBodyParts[i].transform.position = Vector3.Lerp(listBodyParts[i].transform.position,
                    transform.position, bodyFollowDistance);
                listBodyParts[i].transform.up = listBodyParts[i].transform.position - transform.position;
            }
        }
    }

    private void HeadMovement()
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

    private void AddBodyParts()
    {
        // If there is something on List listBodyParts 
        if (listBodyParts.Count != 0)
        {
            // Spawn snakeBodyPrefab into body(snakeBodyPrefab) location that already spawned before
            GameObject snakeBodySpawn = Instantiate(snakeBodyPrefab,
                listBodyParts[listBodyParts.Count - 1].transform.position,
                listBodyParts[listBodyParts.Count - 1].transform.rotation);
            snakeBodySpawn.transform.localScale = transform.localScale;
            // Make sure the earliest sprite(body) spawn layer is behind the old ones
            snakeBodySpawn.GetComponentInChildren<SpriteRenderer>().sortingOrder = (0 - 1 - listBodyParts.Count);

            // Add 
            listBodyParts.Add(snakeBodySpawn.GetComponent<SnakeBody>());

        }
        else
        {
            GameObject snakeBodySpawn = Instantiate(snakeBodyPrefab,
                transform.position,
                transform.rotation);
            snakeBodySpawn.transform.localScale = transform.localScale;
            snakeBodySpawn.GetComponentInChildren<SpriteRenderer>().sortingOrder = (0 - 1 - listBodyParts.Count);

            listBodyParts.Add(snakeBodySpawn.GetComponent<SnakeBody>());
        }
    }
}
