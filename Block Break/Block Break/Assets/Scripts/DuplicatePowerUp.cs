using Unity.VisualScripting;
using UnityEngine;

public class DuplicatePowerUp : MonoBehaviour
{

    public float rotationSpeed = 15f; // Speed of rotation
    public float lifeTime = 50f; // Time before the power-up disappears
    public GameObject ballPrefab; // Reference to the ball prefab
    public GameObject duplicateBallPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Set rotation
        transform.rotation = Quaternion.Euler(90, 0, transform.rotation.eulerAngles.z + rotationSpeed * Time.deltaTime);

        if (lifeTime <= 0)
        {
            Destroy(gameObject); // Destroy the object if lifeTime is less than or equal to 0
        }
        else
        {
            lifeTime -= Time.deltaTime; // Decrease lifeTime by the time since the last frame
        }
    }

void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DuplicateBall(); // Call the method to duplicate the ball
            Destroy(gameObject); // Destroy the power-up object
        }
    }
    void DuplicateBall()
    {
        Ball originalBall = FindFirstObjectByType<Ball>();
        if (originalBall != null && originalBall.spawnPoint != null)

        {
            // Instantiate and configure duplicate
            GameObject newBall = Instantiate(
                duplicateBallPrefab,
                originalBall.spawnPoint.position + new Vector3(5f, 0, 0),
                Quaternion.identity
            );

            Ball newBallScript = newBall.GetComponent<Ball>();
            newBallScript.isOriginal = false; // Mark as non-original
            newBallScript.Launch();
        }
    }

}
