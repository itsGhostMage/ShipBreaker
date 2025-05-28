
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    public float speed = 10f; // Speed of the ball
    public float xAngle = 45f;
    public Rigidbody rb; // Reference to the Rigidbody component
    public Transform spawnPoint;
    public bool startWithDelay = true; // Flag to indicate if the ball should start with a delay

    private float startingX = 0.0f;
    private float startingY = -8.0f;

    public bool gameStart = false;
    public bool relaunch = false;
    public bool isOriginal = true; // Flag to indicate if this is the original ball, mark in inspector

    public static Ball Instance { get; private set; } // Singleton instance
    public bool isPrimaryBall = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component attached to this GameObject
        gameStart = false; // Initialize gameStart to false

        if (startWithDelay)
        {
            StartCoroutine(LaunchBall()); // Start the coroutine to launch the ball after a delay
        }
        else
        {
            Launch(); // Launch the ball immediately
        }
    }

    void Awake()
    {

        // Singleton pattern to ensure only one instance of Ball exists
        if (gameObject.CompareTag("Ball") || !(gameObject.CompareTag("DuplicateBall")))
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void MakeTemporary()
    {
        isPrimaryBall = false;
    }

    void FixedUpdate()
    {
        // Maintain the speed of the ball
        if (gameStart && rb.linearVelocity.magnitude != speed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * speed; 
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lose Zone"))
        {
            if (isOriginal)
            {
                // Only allows the original ball to trigger life loss
                Player player = GameObject.Find("Player").GetComponent<Player>();
                if (player.lives > 1)
                {
                    player.lives--;
                    player.livesText.text = "Lives: " + player.lives;
                    StartCoroutine(ResetBall());
                }
                else
                {
                    StartCoroutine(ReloadSceneCoroutine());
                }
            }
            else
            {
                // Duplicate balls will simply just be destroyed.
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Maintain the speed of the ball after colliding with a ship
        if (collision.gameObject.CompareTag("Ship"))
        {
            rb.linearVelocity = rb.linearVelocity.normalized * speed; 
        }
        
        if (collision.gameObject.CompareTag("Lose Zone"))
        {
            Destroy(this.gameObject);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            // Maintain the speed of the ball after colliding with a player
            rb.linearVelocity = rb.linearVelocity.normalized * speed;
        }
    }

    IEnumerator LaunchBall()
    {
        // Wait for Countdown to complete before launching the ball
        yield return new WaitForSeconds(3f);
        Launch();
    }

    public void Launch()
    {
        // Calculate direction with xAngle constraints
        float tanAngle = Mathf.Tan(xAngle * Mathf.Deg2Rad);

        // Randomize the direction of the ball
        Vector3 direction = new Vector3(Random.Range(tanAngle, -tanAngle), 1).normalized;

        // Apply angular velocity to the ball
        rb.linearVelocity = direction * speed; 

        gameStart = true;
        speed = 15f; // Reset speed in case SpeedUp Power-Up was collected before relaunching
    }


    IEnumerator ResetBall()
    {
        // Reset the ball's state
        gameStart = false;
        rb.linearVelocity = Vector3.zero;
        transform.position = new Vector3(startingX, startingY, 0);
        Launch();
        yield return null;
    }

    IEnumerator ReloadSceneCoroutine()
    {
        // Reload the current scene after a 1-second delay
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }
}
