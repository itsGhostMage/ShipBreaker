using UnityEngine;

public class TriangePlatform : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Laser"))
        {
            // Destroy the laser
            Destroy(collision.gameObject);
        }
    }
}
