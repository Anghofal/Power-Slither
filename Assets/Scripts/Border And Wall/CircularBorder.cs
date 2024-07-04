using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularBorder : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab; // Prefab with BoxCollider2D
    [SerializeField] int numberOfSegments; // Number of box colliders to form the circle
    public static float radius = 16.8f;
    public static List<InvisibleBox> listInvisibleBox = new List<InvisibleBox>();
    // Start is called before the first frame update

    // Var for food spawn
    [SerializeField] private float foodSpawnTime;
    [SerializeField] private GameObject foodPrefabs;
    public static List<Apple> listApples = new List<Apple>();
    private int maxFoodSpawn = 100;

    // var for body spawn
    //private float bodySpawnTime = 4f;
    [SerializeField] private GameObject bodyPrefabs;
    //public static List<SnakeBody> listRandomBodySpawn = new List<SnakeBody>();
    //private int maxBodyRandomSpawn = 16;
    
    //
    [SerializeField] private GameObject snakeHeadPrefabs;
    public static List<SnakeMoveAgent> listSnakeHead = new List<SnakeMoveAgent>();

    private void Start()
    {
        CreateCircularBorder();
        
        for (int i = 0; i < 4; i++)
        {
            Vector2 randomPos = createRandomPos();

            // Instantiate food at the calculated position
            GameObject appleFood = Instantiate(foodPrefabs, randomPos, Quaternion.identity);

            listApples.Add(appleFood.GetComponent<Apple>());
        }
        
        // spawn new head
        for (int i = 1; i <= 6; i++)
        {
            Vector2 randomPos = createRandomPos();

            GameObject snakeGameObject = Instantiate(snakeHeadPrefabs, randomPos, Quaternion.identity);
            snakeGameObject.GetComponent<SnakeMoveAgent>().headNumberIdentifier += i;
            listSnakeHead.Add(snakeGameObject.GetComponent<SnakeMoveAgent>());
        }

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        
        if (listApples.Count <= maxFoodSpawn)
        {
            StartCoroutine("SpawnFood");
        }
        /*if (listRandomBodySpawn.Count <= maxBodyRandomSpawn)
        {
            StartCoroutine("SpawnBody");
        }*/
    }

    private void CreateCircularBorder()
    {
        float angleStep = 360f / numberOfSegments;

        for (int i = 0; i < numberOfSegments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            // Calculate position and rotation
            Vector2 position = new Vector2(x, y);
            float rotation = angle * Mathf.Rad2Deg;

            // Instantiate and configure wall segment
            GameObject wallSegment = Instantiate(wallPrefab, position, Quaternion.Euler(0, 0, rotation), transform);
            listInvisibleBox.Add(wallSegment.GetComponent<InvisibleBox>());
        }
    }

    /*private IEnumerator SpawnFood()
    {
        yield return new WaitForSeconds(foodSpawnTime);

        Vector2 RandomPos = new Vector2(UnityEngine.Random.Range(transform.position.x - 8, transform.position.x + 8), UnityEngine.Random.Range(transform.position.y - 8, transform.position.y + 8));

        GameObject appleFood = Instantiate(foodPrefabs, RandomPos, Quaternion.identity);

        listApples.Add(appleFood.GetComponent<Apple>());

        StopCoroutine("SpawnFood");
    }*/

    public static Vector2 createRandomPos()
    {
        
        // Generate a random angle and distance
        float randomAngle = Random.Range(0f, Mathf.PI * 2);
        float randomRadius = Random.Range(0f, radius);

        // Calculate the random position within the circle
        float x = Mathf.Cos(randomAngle) * randomRadius;
        float y = Mathf.Sin(randomAngle) * randomRadius;
        
        Vector2 randomPos = new Vector2(x, y);
        return randomPos;
    }

    /*public IEnumerator SpawnBody()
    {
        if (listRandomBodySpawn.Count == 0)
        {
            for (int i = 0; i < maxBodyRandomSpawn; i++) {
                Vector2 randomPos = createRandomPos();

                // Instantiate food at the calculated position
                GameObject bodySpawnPrefab = Instantiate(bodyPrefabs, randomPos, Quaternion.identity);

                listRandomBodySpawn.Add(bodySpawnPrefab.GetComponent<SnakeBody>());
            }
        }
        yield return new WaitForSeconds(bodySpawnTime);

        foreach (var snakeBodyRandomedSpawn in listRandomBodySpawn)
        {
            Destroy(snakeBodyRandomedSpawn.gameObject);
        }
        listRandomBodySpawn.Clear();

        StopCoroutine("SpawnBody");
    }*/

    public IEnumerator SpawnFood()
    {
        yield return new WaitForSeconds(foodSpawnTime);

        /*
        // Generate a random angle and distance
        float randomAngle = UnityEngine.Random.Range(0f, Mathf.PI * 2);
        float randomRadius = UnityEngine.Random.Range(0f, radius);

        // Calculate the random position within the circle
        float x = Mathf.Cos(randomAngle) * randomRadius;
        float y = Mathf.Sin(randomAngle) * randomRadius;
        */
        Vector2 randomPos = createRandomPos();
        
        // Instantiate food at the calculated position
        GameObject appleFood = Instantiate(foodPrefabs, randomPos, Quaternion.identity);

        listApples.Add(appleFood.GetComponent<Apple>());

        StopCoroutine("SpawnFood");
    }
}
