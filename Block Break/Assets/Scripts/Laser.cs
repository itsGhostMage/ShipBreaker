using UnityEngine;

public class Laser : MonoBehaviour
{

    public float laserSpeed = 10f;
    public float yEdge = -20f; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // lock to the Z-Axis
        transform.position = new(transform.position.x, transform.position.y, 0);
        transform.Translate(Vector3.down * Time.deltaTime * laserSpeed);

        if (transform.position.y < yEdge)
        {
            Destroy(gameObject); // Destroy the laser object if it goes out of bounds
        }

        // Ignore collisions with the ball
        Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.Find("Ball").GetComponent<Collider>());
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject); // Destroy the laser object upon collision with the player


            Player player = collision.gameObject.GetComponent<Player>(); // Get the Player component from the collided object
            if (player == null)
            {
                Debug.LogError("Player component not found on the collided object.");
                return;
            }

            if (player.lives >= 1)
            {
                player.lives--; // Decrease the player's lives
                player.livesText.text = "Lives: " + player.lives;
            }
        }
    }

}
