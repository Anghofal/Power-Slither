using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularBorder : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab; // Prefab with BoxCollider2D
    [SerializeField] private int numberOfSegments; // Number of box colliders to form the circle
    [SerializeField] private float radius;
    // Start is called before the first frame update

    // Var for eating
    [SerializeField] private float foodSpawnTime;
    [SerializeField] private GameObject foodPrefabs;
    public static List<Apple> listApples = new List<Apple>();
    private int maxFoodSpawn = 100;
    private void Start()
    {
        CreateCircularBorder(); 
    }

    // Update is called once per frame
    private void Update()
    {
        
        if (listApples.Count <= maxFoodSpawn)
        {
            StartCoroutine("SpawnFood");
        }   
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

    private IEnumerator SpawnFood()
    {
        yield return new WaitForSeconds(foodSpawnTime);

        // Generate a random angle and distance
        float randomAngle = UnityEngine.Random.Range(0f, Mathf.PI * 2);
        float randomRadius = UnityEngine.Random.Range(0f, radius);

        // Calculate the random position within the circle
        float x = Mathf.Cos(randomAngle) * randomRadius;
        float y = Mathf.Sin(randomAngle) * randomRadius;
        Vector2 randomPos = new Vector2(x, y);

        // Instantiate food at the calculated position
        GameObject appleFood = Instantiate(foodPrefabs, randomPos, Quaternion.identity);

        listApples.Add(appleFood.GetComponent<Apple>());

        StopCoroutine("SpawnFood");
    }
}
