using UnityEngine;

public class PointSpinner : MonoBehaviour
{

    public float rotationSpeed = 15; // Speed of rotation in degrees per second

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = transform.rotation * Quaternion.Euler(-rotationSpeed * Time.deltaTime, 0, 0); // Rotate the object
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Laser"))
        {
            // Destroy the laser
            Destroy(collision.gameObject);
        }
    }
}
