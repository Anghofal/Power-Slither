using System;
using System.Collections;
using System.Collections.Generic;
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

    // Var for eating
    [SerializeField] private float foodSpawnTime;
    [SerializeField] private GameObject foodPrefabs;
    private List<Apple> listApples = new List<Apple>();

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

        StartCoroutine("SpawnFood");
        
    }
    
    private void BodyMovement()
    {
        for (int i = 0; i < listBodyParts.Count; i++)
        {
            // this is movement script for the second and more body
            if (i != 0)
            {
                // Move the second body location into first body location but with lerp 
                listBodyParts[i].transform.position = Vector3.Lerp(listBodyParts[i].transform.position,
                    listBodyParts[i - 1].transform.position, bodyFollowDistance);
                // Turn rotation of Z Axis (green Axis) the second body like the first body  
                listBodyParts[i].transform.up = listBodyParts[i].transform.position - listBodyParts[i - 1].transform.position;
            }
            // this is movement script for the first body
            else
            {
                // Move the first body location into head location but with lerp 
                listBodyParts[i].transform.position = Vector3.Lerp(listBodyParts[i].transform.position,
                    transform.position, bodyFollowDistance);
                // Turn rotation of Z Axis (green Axis) the second body like the first body 
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

    private IEnumerator SpawnFood()
    {
        yield return new WaitForSeconds(foodSpawnTime);
        
        Vector2 RandomPos = new Vector2(UnityEngine.Random.Range(transform.position.x - 8, transform.position.x + 8), UnityEngine.Random.Range(transform.position.y - 8, transform.position.y + 8));

        GameObject appleFood = Instantiate(foodPrefabs, RandomPos, Quaternion.identity);
        

        StopCoroutine("SpawnFood");
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Apple applePrefabs = col.gameObject.GetComponent<Apple>();
        if (applePrefabs != null)
        {
            Destroy(col.gameObject);
            AddBodyParts();
        }
    }
}
