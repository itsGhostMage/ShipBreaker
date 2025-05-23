using UnityEngine;

public class DupBall : MonoBehaviour
{

    public float speed = 10f; // Speed of the ball
    public float xAngle = 45f;
    public Rigidbody rb; // Reference to the Rigidbody component
    public Ball ballPrefab; // Reference to the Ball prefab

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude != speed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * speed; // Maintain the speed of the ball
        }
    }

    public void Launch()
    {
        // Calculate direction with xAngle constraints
        float tanAngle = Mathf.Tan(xAngle * Mathf.Deg2Rad);
        Vector3 direction = new Vector3(Random.Range(tanAngle, -tanAngle), 1).normalized; // Randomize the direction of the ball
        rb.linearVelocity = direction * speed; // Apply angular velocity to the ball

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ship"))
        {
            rb.linearVelocity = rb.linearVelocity.normalized * speed; // Maintain the speed of the ball
        }

        if (collision.gameObject.CompareTag("Lose Zone"))
        {
            Destroy(this.gameObject);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            // Maintain the speed of the ball
            if (rb.linearVelocity.magnitude != speed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * speed;
            }
        }
    }

    void CreateExtraBall()
    {
        Ball newBall = Instantiate(ballPrefab);
        // Mark as non-primary
        newBall.MakeTemporary(); 
    }
}
