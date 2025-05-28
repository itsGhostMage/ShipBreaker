using UnityEngine;

public class Brick : MonoBehaviour
{
    public int collisionIndex;
    public Material[] materialList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (materialList.Length == 0)
        {
            Debug.LogError("Material list is empty. Please assign materials in the inspector.");
            return;
        }
    }

    // OnCollisionEnter is called when the collider attached to this object collides with another collider
    void OnCollisionEnter(Collision collision)
    {
        // Check if player collided with the object
        if (collision.gameObject.CompareTag("Ball"))
        {
            collisionIndex++; // Increment the collision index

            // Check the bounds BEFORE accessing the array
            if (collisionIndex < materialList.Length)
            {
                GetComponent<Renderer>().material = materialList[collisionIndex];
            }
            else
            {
                Destroy(gameObject,  0.1f); // Destroy the object if the index exceeds the length of the material list
                Debug.Log("Collision index out of bounds. Destroying object.");
            }
        }
    }
}
