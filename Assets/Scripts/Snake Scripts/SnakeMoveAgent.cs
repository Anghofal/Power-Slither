using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Xml.Schema;
using System.Drawing;
public class SnakeMoveAgent : Agent
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
    public List<SnakeBody> listBodyParts = new List<SnakeBody>();
    [SerializeField] private float bodyFollowDistance;

    // var for body collision
    public int headNumberIdentifier;
    public static bool isSnakeCollidingWithBorder = false;

    // var for movement
    private Vector2 snakeVector2;
    
    // var for reference
    private SnakeHead snakeHead;
    [SerializeField] private GameObject foodPrefabs;
    [SerializeField] private GameObject snakeHeadPrefabs;
    //[SerializeField] GameInput gameInput;

    // var for sensor
    private Vector2 snakeMoveDir;

    //
    public static Apple nearestApple;
    public static SnakeHead nearestHead;
    public static SnakeBody nearestBody;
    public static InvisibleBox nearestInvisbleBox;

    private void Start()
    {
        AddBodyParts();
        AddBodyParts();
        AddBodyParts();
    }

    public override void Initialize()
    {
        // Initialize variables or components here
        //snakeHead = GetComponent<SnakeHead>();
        
        /*for (int i = 0; i < 4; i++)
        {
            Vector2 randomPos = CircularBorder.createRandomPos();

            // Instantiate food at the calculated position
            GameObject appleFood = Instantiate(foodPrefabs, randomPos, Quaternion.identity);

            CircularBorder.listApples.Add(appleFood.GetComponent<Apple>());
        }*/

        //Time.timeScale = 0.5f;
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        //Debug.Log($"Action 0: {actions.ContinuousActions[0]}, Action 1: {actions.ContinuousActions[1]}");
        snakeVector2 = new Vector2(actions.ContinuousActions[0], actions.ContinuousActions[1]);
        HeadMovement(snakeVector2);
        
        /*if (CircularBorder.listApples.Count == 0)
        {
            EndEpisode();
        }*/

        /*// Optionally, you can also end the episode if a negative threshold is reached
        if (GetCumulativeReward() <= -6f) // Example condition: cumulative reward <= -10
        {
            EndEpisode();
        }*/
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("Episode started");
        // Reset the agent or environment at the start of each episode

        /*
        foreach (var snakeBody in snakeHead.listBodyParts)
        {
            Destroy(snakeBody.gameObject);
        }
        snakeHead.listBodyParts.Clear();
        */

        // Delte all apples in environment and list
        /*foreach (var apple in CircularBorder.listApples)
        {
            Destroy(apple.gameObject);
        }
        CircularBorder.listApples.Clear();

        //
        for (int i = 0; i < 4; i++)
        {
            Vector2 randomPos = CircularBorder.createRandomPos();

            // Instantiate food at the calculated position
            GameObject appleFood = Instantiate(foodPrefabs, randomPos, Quaternion.identity);

            CircularBorder.listApples.Add(appleFood.GetComponent<Apple>());
        };

        foreach (var snakeHead in CircularBorder.listSnakeHead)
        {
            if (snakeHead.listBodyParts.Count >= 4)
            {
                for (int i = 4; i <= snakeHead.listBodyParts.Count; i++)
                {
                    Destroy(snakeHead.listBodyParts[i-1].gameObject);
                    snakeHead.listBodyParts.RemoveAt(i-1);
                }
            }
            snakeHead.transform.position = CircularBorder.createRandomPos();
        }*/
        

        

        //transform.position = CircularBorder.createRandomPos();
    }

    

    public override void CollectObservations(VectorSensor sensor)
    {
        // Vector Observation +2 (analyze direction of head)
        snakeMoveDir = new Vector2(snakeLastMoveDir.x, snakeLastMoveDir.y);
        sensor.AddObservation(snakeMoveDir);
        //Debug.Log($"Observation snakeMoveDir: {snakeMoveDir}");

        
        nearestApple = GetNearestApple();
        if (nearestApple != null)
        {
            // Vector Observation +2 (analyze direction to nearest apple)
            Vector2 directionToApple = (nearestApple.transform.position - transform.position).normalized;
            sensor.AddObservation(directionToApple);
            
            // Vector Observation +1 (analyze distance to nearest apple)
            float distanceToApple = Vector3.Distance(transform.position, nearestApple.transform.position);
            sensor.AddObservation(distanceToApple);

            //Debug.Log($"Observation directionToApple: {directionToApple}, distanceToApple: {distanceToApple}");
        }
        else
        {
            sensor.AddObservation(Vector2.zero);
            sensor.AddObservation(0f);
            Debug.Log("No apple found, default observations added.");
        }

        /*
        nearestHead = GetNearestHead();
        if (nearestHead != null)
        {
            // Vector Observation +2 (analyze direction to nearest snake head)
            Vector2 directionToHead = (nearestHead.transform.position - transform.position).normalized;
            sensor.AddObservation(directionToHead);

            // Vector Observation +1 (analyze distance to nearest snake head)
            float distanceToHead = Vector3.Distance(transform.position, nearestHead.transform.position);
            sensor.AddObservation(distanceToHead);

            
        }
        else
        {
            sensor.AddObservation(Vector2.zero);
            sensor.AddObservation(0f);
            Debug.Log("No head found, default observations added.");
        }

        //
        nearestBody = GetNearestBody();
        if (nearestBody != null)
        {
            // Vector Observation +2 (analyze direction to nearest snake body)
            Vector2 directionToBody = (nearestBody.transform.position - transform.position).normalized;
            sensor.AddObservation(directionToBody);

            // Vector Observation +1 (analyze distance to nearest apple)
            float distanceToBody = Vector3.Distance(transform.position, nearestBody.transform.position);
            sensor.AddObservation(distanceToBody);


        }
        else
        {
            sensor.AddObservation(Vector2.zero);
            sensor.AddObservation(0f);
            Debug.Log("No body found, default observations added.");
        }
        */

        /*
        nearestInvisbleBox = GetNearestBorder();
        if (nearestInvisbleBox != null)
        {
            // Vector Observation +2 (analyze direction to nearest border)
            Vector2 directionToBorder = (nearestInvisbleBox.transform.position - transform.position).normalized;
            sensor.AddObservation(directionToBorder);

            // Vector Observation +1 (analyze distance to nearest border)
            float distanceToBorder = Vector3.Distance(transform.position, nearestInvisbleBox.transform.position);
            sensor.AddObservation(distanceToBorder);
        }
        else
        {
            sensor.AddObservation(Vector2.zero);
            sensor.AddObservation(0f);
            Debug.Log("No border found, default observations added.");
        }
        */
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Provide heuristic actions for manual control
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");
    }

    private Vector2 whenSnakeIsCollisionBorder()
    {
        Vector2 snakeVector2 = Vector2.zero;

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
        return snakeVector2;
    }

    private void SmoothHeadMovement(Vector2 snakeVector2)
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

    private void HeadMovement(Vector2 snakeVector2)
    {

        // Make new var Distance later we will multiply with snakeMoveDir
        // and to know how far snake will go over seconds using movement speed parameter
        float snakeMoveDistance = snakeMovSpeed * Time.deltaTime;

        if (isSnakeCollidingWithBorder)
        {

            snakeVector2 = whenSnakeIsCollisionBorder();

            SmoothHeadMovement(snakeVector2);

            // Move position head based on move direction (input) and distance (speed) 
            transform.position += snakeLastMoveDir * snakeMoveDistance;
        }
        else
        {
            if (snakeVector2 != Vector2.zero)
            {
                SmoothHeadMovement(snakeVector2);
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

    public void AddRewardToAgent(float reward)
    {
        AddReward(reward);
    }

    public void EndAgentEpisode()
    {
        EndEpisode();
    }

    public InvisibleBox GetNearestBorder()
    {
        InvisibleBox nearestBorder = null;
        float shortestDistance = Mathf.Infinity;

        foreach (InvisibleBox invisibleBox in CircularBorder.listInvisibleBox)
        {
            float distanceToInvisibleBox = Vector3.Distance(transform.position, invisibleBox.transform.position);
            if (distanceToInvisibleBox < shortestDistance)
            {
                shortestDistance = distanceToInvisibleBox;
                nearestBorder = invisibleBox;
            }
        }
        return nearestBorder;
    }

    public SnakeMoveAgent GetNearestHead()
    {
        SnakeMoveAgent nearestHead = null;
        float shortestDistance = Mathf.Infinity;

        foreach (SnakeMoveAgent snakeMoveAgent in CircularBorder.listSnakeHead)
        {
            float distanceToSnakeHead = Vector3.Distance(transform.position, snakeHead.transform.position);
            if (distanceToSnakeHead < shortestDistance)
            {
                shortestDistance = distanceToSnakeHead;
                nearestHead = snakeMoveAgent;
            }
        }
        return nearestHead;
    }

    public SnakeBody GetNearestBody()
    {
        SnakeBody nearestBody = null;
        float shortestDistance = Mathf.Infinity;
        SnakeMoveAgent nearestSnakeHead = GetNearestHead();
        if (nearestSnakeHead != null)
        {
            foreach (SnakeBody snakeBody in nearestSnakeHead.GetComponent<SnakeHead>().listBodyParts)
            {
                float distanceToSnakeBody = Vector3.Distance(transform.position, snakeBody.transform.position);
                if (distanceToSnakeBody < shortestDistance)
                {
                    shortestDistance = distanceToSnakeBody;
                    nearestBody = snakeBody;
                }
            }
        }
        return nearestBody;
    }

    public Apple GetNearestApple()
    {
        Apple nearestApple = null;
        float shortestDistance = Mathf.Infinity;


        foreach (Apple apple in CircularBorder.listApples)
        {
            float distanceToApple = Vector3.Distance(transform.position, apple.transform.position);
            if (distanceToApple < shortestDistance)
            {
                shortestDistance = distanceToApple;
                nearestApple = apple;
            }
        }

        return nearestApple;
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

            snakeBodySpawn.GetComponent<SnakeBody>().bodyNumberIdentifier = headNumberIdentifier;

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

            snakeBodySpawn.GetComponent<SnakeBody>().bodyNumberIdentifier = headNumberIdentifier;

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

            Apple collidedApple = col.GetComponent<Apple>();
            if (collidedApple != nearestApple)
            {
                Debug.Log("Agent Minus Reward not eating the right apple");
                //snakeMoveAgent.AddRewardToAgent(-1f);
            }
            else
            {
                Debug.Log("Agent plus Reward eating the nearest apple");
                // when agent is eating the nearest apple add reward
                AddRewardToAgent(+4f);
                //snakeMoveAgent.EndAgentEpisode();
            }
            // Existing logic for eating food
            CircularBorder.listApples.Remove(col.GetComponent<Apple>());
            Destroy(col.gameObject);

            if (listBodyParts.Count <= 15)
            {
                AddBodyParts();
            }
        }
        else if (col.GetComponent<InvisibleBox>() != null)
        {
            // when agent is facing border minus reward
            AddRewardToAgent(-1f);
            //snakeMoveAgent.EndAgentEpisode();

            isSnakeCollidingWithBorder = true;
        }
        
        else if (col.GetComponent<SnakeBody>() != null)
        {
            if (col.GetComponent<SnakeBody>().bodyNumberIdentifier != headNumberIdentifier)
            {
                foreach (var snakeBodyParts in listBodyParts)
                {
                    Destroy(snakeBodyParts.gameObject);
                }
                Destroy(gameObject);
                // when agent is facing npc body minus reward
                AddRewardToAgent(-2f);
                //snakeMoveAgent.EndAgentEpisode();

            }
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.GetComponent<InvisibleBox>() != null)
        {
            //snakeMoveAgent.AddReward(-1f);
            isSnakeCollidingWithBorder = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<InvisibleBox>() != null)
        {
            // when the agent is exit the border add reward
            //snakeMoveAgent.AddRewardToAgent(+1f);

            float snakeMoveDistance = snakeMovSpeed * Time.deltaTime;
            Vector2 snakeVector2 = whenSnakeIsCollisionBorder();

            SmoothHeadMovement(snakeVector2);

            transform.position += snakeLastMoveDir * snakeMoveDistance;

            isSnakeCollidingWithBorder = false;
        }
    }
}
