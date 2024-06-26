using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Snakehead : MonoBehaviour
{
    // var for movement head
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float snakeMovSpeed;
    [SerializeField] private float snakeRotationSensivity;
    private Vector3 snakeLastMoveDir;

    // var for smoothing movement
    [SerializeField] private float inputSmoothingFactor;
    private Vector2 smoothedInputVector2 = Vector2.up;
    [SerializeField] private float minimumMovementThreshold;

    // var for body movement
    [SerializeField] private GameObject snakeBodyPrefab;
    private List<SnakeBody> listBodyParts = new List<SnakeBody>();
    [SerializeField] private float bodyFollowDistance;

    // Var for body collision
    [SerializeField] private int headNumberIdentifier = 0;

    // var for border detection
    private bool isCollidingWithBorder = false;
    /*private Vector3 borderCenter = new Vector3(0, 0, 0);
    [SerializeField] private float borderRadius;*/

    

    private void Awake()
    {

    }

    // Start is called before the first frame update
    private void Start()
    {
        snakeLastMoveDir = new Vector3(0, 2, 0);
        
    }

    // Update is called once per frame
    private void Update()
    {
        HeadMovement();
        HandleInteractions();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddBodyParts();
        }

        

    }

    private void HeadMovement()
    {
        // Get Vector2 from GameInput Class
        Vector2 snakeVector2 = gameInput.GetMovementVector2Normalized();

        // Make new var Distance later we will multiply with snakeMoveDir
        // and to know how far snake will go over seconds using movement speed parameter
        float snakeMoveDistance = snakeMovSpeed * Time.deltaTime;

        if (isCollidingWithBorder)
        {
            if (transform.position.x <= 0 && transform.position.y <= 0)
            {
                snakeVector2 = new Vector2(1, 1).normalized;

            }
            else if (transform.position.x <= 0 && transform.position.y >= 0)
            {
                snakeVector2 = new Vector3(1, -1).normalized;
            }
            else if (transform.position.x >= 0 && transform.position.y <= 0)
            {
                snakeVector2 = new Vector3(-1, 1).normalized;
            }
            else
            {
                snakeVector2 = new Vector3(-1, -1).normalized;
            }

            SmoothHeadMovement(snakeVector2, snakeMoveDistance);

            // Move position head based on move direction (input) and distance (speed) 
            transform.position += snakeLastMoveDir * snakeMoveDistance;
        }
        else
        {
            if (snakeVector2 != Vector2.zero)
            {
                SmoothHeadMovement(snakeVector2, snakeMoveDistance);
            }

            // Move position head based on move direction (input) and distance (speed) 
            transform.position += snakeLastMoveDir * snakeMoveDistance;
        }

        // Making rotation based on move direction
        Quaternion snakeDir = Quaternion.LookRotation(Vector3.forward, snakeLastMoveDir);

        // Make head rotation to
        transform.rotation = Quaternion.Lerp(transform.rotation, snakeDir, snakeRotationSensivity * Time.deltaTime);

        for (int i = 0; i < listBodyParts.Count; i++)
        {
            // this is movement script for the second and more body
            if (i != 0)
            {
                // Move the second body location into first body location but with lerp 
                listBodyParts[i].transform.position = Vector3.Lerp(listBodyParts[i].transform.position,
                    listBodyParts[i - 1].transform.position, bodyFollowDistance * Time.deltaTime);
                // Turn rotation of Z Axis (green Axis) the second body like the first body  
                listBodyParts[i].transform.up = listBodyParts[i].transform.position - listBodyParts[i - 1].transform.position;
            }
            // this is movement script for the first body
            else
            {
                // Move the first body location into head location but with lerp 
                listBodyParts[i].transform.position = Vector3.Lerp(listBodyParts[i].transform.position,
                    transform.position, bodyFollowDistance * Time.deltaTime);
                // Turn rotation of Z Axis (green Axis) the second body like the first body 
                listBodyParts[i].transform.up = listBodyParts[i].transform.position - transform.position;
            }
        }
    }

    private void SmoothHeadMovement(Vector2 snakeVector2, float snakeMoveDistance)
    {
        // Apply low-pass filter to smooth the input
        smoothedInputVector2 = Vector2.Lerp(smoothedInputVector2, snakeVector2, inputSmoothingFactor * Time.deltaTime);

        // If there is significant input, update the move direction
        if (smoothedInputVector2.magnitude < minimumMovementThreshold)
        {
            smoothedInputVector2 = smoothedInputVector2.normalized * minimumMovementThreshold;
        }

        // Update the last move direction to ensure continuous movement
        snakeLastMoveDir = new Vector3(smoothedInputVector2.x, smoothedInputVector2.y, 0);
    }

    private void HandleInteractions()
    {

        float interactDistance = 0.3f;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, interactDistance);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out SnakeBody snakeBody))
            {
                if (this.headNumberIdentifier == snakeBody.bodyNumberIdentifier)
                {
                }
                else
                {
                    Debug.Log("Ini bukan body ku");
                    Destroy(this.gameObject);
                    foreach (SnakeBody bodySnake in listBodyParts)
                    {
                        Destroy(bodySnake.gameObject);
                    }
                }
            }
        }

        /*
        // Get Vector2 from GameInput Class
        Vector2 snakeVector2 = gameInput.GetMovementVector2Normalized();

        // Asign the vector from var snakeVector2 into snakeMoveDirection
        Vector3 snakeMoveDir = new Vector3(snakeVector2.x, snakeVector2.y, 0); 

        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, snakeMoveDir, interactDistance);

        if (raycastHit.collider != null)
        {
            if (raycastHit.transform.TryGetComponent(out SnakeBody snakeBody))
            {
                if (this.snakehead.headNumberIdentifier == snakeBody.bodyNumberIdentifier)
                {
                    Debug.Log("Ya ini body ku");
                }
                else
                {
                    Debug.Log("Ini bukan body ku");
                    Destroy(this.snakehead.gameObject);
                    foreach (SnakeBody bodySnake in listBodyParts)
                    {
                        Destroy(bodySnake.gameObject);
                    }
                    
                }
            }
        }*/

    }

    private void AddBodyParts()
    {
        // If there is something on List listBodyParts 
        if (listBodyParts.Count != 0)
        {
            // Spawn snakeBodyPrefab into body(snakeBodyPrefab) location that spawned before this body
            GameObject snakeBodySpawn = Instantiate(snakeBodyPrefab,
                listBodyParts[listBodyParts.Count - 1].transform.position,
                listBodyParts[listBodyParts.Count - 1].transform.rotation);
            snakeBodySpawn.transform.localScale = transform.localScale;
            // Make sure the earliest sprite(body) spawn layer is behind the old ones
            snakeBodySpawn.GetComponentInChildren<SpriteRenderer>().sortingOrder = (0 - 1 - listBodyParts.Count);

            // Add the body reference to List
            listBodyParts.Add(snakeBodySpawn.GetComponent<SnakeBody>());

        }
        else
        {
            // Spawn snakeBodyPrefab into head game object location
            GameObject snakeBodySpawn = Instantiate(snakeBodyPrefab,
                transform.position,
                transform.rotation);
            snakeBodySpawn.transform.localScale = transform.localScale;
            // Make sure the earliest sprite(body) spawn layer is behind the head
            snakeBodySpawn.GetComponentInChildren<SpriteRenderer>().sortingOrder = (0 - 1 - listBodyParts.Count);

            // Add the body reference to List
            listBodyParts.Add(snakeBodySpawn.GetComponent<SnakeBody>());
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.GetComponent<Apple>() != null)
        {
            Debug.Log(CircularBorder.listApples.Count);
            // Existing logic for eating food
            CircularBorder.listApples.Remove(col.GetComponent<Apple>());
            Destroy(col.gameObject);
            Debug.Log("An apple is destroyed");
            Debug.Log(CircularBorder.listApples.Count);
            AddBodyParts();
        }
        else if (col.gameObject.tag == "InvisibleWall")
        {
            isCollidingWithBorder = true;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "InvisibleWall")
        {
            isCollidingWithBorder = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "InvisibleWall")
        {
            isCollidingWithBorder = false;
        }
    }

    /*void ConstrainWithinBorder()
    {
        Vector3 snakePosition = transform.position;
        Vector3 centerToSnake = snakePosition - borderCenter;
        float distanceFromCenter = centerToSnake.magnitude;

        if (distanceFromCenter > borderRadius)
        {
            Vector3 directionToCenter = centerToSnake.normalized;
            Vector3 newPosition = borderCenter + directionToCenter * borderRadius;
            transform.position = newPosition;
        }
    }*/

}
