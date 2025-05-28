using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ship : MonoBehaviour
{
    public int lives;
    public float speed = 4f;

    public Material Blue;
    public Material Green;
    public Material Yellow;
    public Material Orange;
    public Material Red;

    [SerializeField] Transform firePoint; // Reference to the fire point for laser instantiation
    [SerializeField] Transform firePoint2; // Reference to the second fire point for laser instantiation
    [SerializeField] GameObject laser;
    public float laserSpeed = 10f;

    public float timeToAdd;

    public float minWait;
    public float maxWait;

    public int scoreToAdd = 150;

    public bool isGameOver = true; // Flag to indicate if the game is over
    public bool noLasersCollected = false; // Flag to indicate if a No Lasers Powerup has been collected

    IEnumerator Start()
    {

        isGameOver = false; // Set the game over flag to false

        StartCoroutine(RandomShoot()); // Begin firing lasers

        yield return null;

        if (lives == 1)
        {
            scoreToAdd = 100; // Set score to add based on lives
        }
        else if (lives == 2)
        {
            scoreToAdd = 150;
        }
        else if (lives == 3)
        {
            scoreToAdd = 200;
        }
        else if (lives == 4)
        {
            scoreToAdd = 250;
        }
        else if (lives == 5)
        {
            scoreToAdd = 300;
        }

    }

    // Add this to ensure lasers get re-enabled even if ships are destroyed/respawned
    void OnEnable()
    {
        noLasersCollected = false;
    }


    void Update()
    {
        if (lives <= 0)
        {
            Destroy(gameObject, 0.1f); // Destroy the object if lives are less than or equal to 0
        }
        else if (lives == 1)
        {
            GetComponent<Renderer>().material = Blue;
        }
        else if (lives == 2)
        {
            GetComponent<Renderer>().material = Green;
        }
        else if (lives == 3)
        {
            GetComponent<Renderer>().material = Yellow;
        }
        else if (lives == 4)
        {
            GetComponent<Renderer>().material = Orange;
        }
        else if (lives == 5)
        {
            GetComponent<Renderer>().material = Red;
        }
    }

    void OnDestroy()
    {
        SpawnManager spawnManager = FindFirstObjectByType<SpawnManager>();
        if (spawnManager != null)
        {
            spawnManager.RemoveEnemy(gameObject);
        }
    }

    IEnumerator RandomShoot()
    {
        // Wait for ball initialization
        yield return new WaitUntil(() => Ball.Instance != null);

        Rigidbody ballRb = Ball.Instance.GetComponent<Rigidbody>();


        while (!isGameOver && ballRb != null)
        {
            // Only shoot when ball is moving
            if (ballRb.linearVelocity.magnitude > 0.1f)
            {
                // Movement
                transform.Translate(Vector3.down * Time.deltaTime * speed);

                // Shooting delay
                yield return new WaitForSeconds(Random.Range(minWait, maxWait));

                FireLasers();
            }
            yield return null;
        }
    }

    void FireLasers()
    {
        if (laser == null) return;

        if (noLasersCollected != true)
        {
            // First laser
            InstantiateLaser(firePoint);
            // Second laser if available
            if (firePoint2 != null)
            {
                InstantiateLaser(firePoint2);
            }
        }
    }

    void InstantiateLaser(Transform point)
    {

        GameObject laserInstance = Instantiate(
            laser,
            point.position,
            point.rotation
        );

        Rigidbody rb = laserInstance.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.down * laserSpeed, ForceMode.Impulse);
    }

    // OnCollisionEnter is called when the collider attached to this object collides with another collider
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball") || (collision.gameObject.CompareTag("DuplicateBall")))
        {
            lives--;
            if (lives == 0)
            {
                Destroy(gameObject, 0.1f);

                // Access the timer through the singleton
                if (Timer.Instance != null)
                {
                    Timer.Instance.AddTime(timeToAdd);
                }
                
                if (Score.Instance != null)
                {
                    Score.Instance.AddScore(scoreToAdd); // Add score to the player
                }
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>(); // Get the Player component from the collided object
            if (player == null)
            {
                return;
            }
                player.lives--; // Decrement player lives

                // Destory the laser on collision with player
                Destroy(collision.gameObject); // Destroy the laser object
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("TargetZone"))
        {
            StartCoroutine(ReloadSceneCoroutine());
        }
    }

    IEnumerator ReloadSceneCoroutine()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
        isGameOver = true; // Set the game over flag to true
    }
}


