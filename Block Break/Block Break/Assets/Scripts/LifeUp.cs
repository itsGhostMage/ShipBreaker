using UnityEngine;
using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

public class LifeUp : MonoBehaviour
{
    public float spinSpeed = 15f;
    public float lifeTimeTimer = 50f; // Time before the object is destroyed

    void Start()
    {

    }

    void Update()
    {
        // Rotate along the Z-Axis at the rate of spinSpeed
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);


        lifeTimeTimer -= Time.deltaTime; // Decrease the timer by the time passed since last frame

        if (lifeTimeTimer <= 0)
        {
            // Destroy the object if the timer reaches zero
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.CompareTag("Player")))
        {
            Player player = other.gameObject.GetComponent<Player>(); // Get the Player component from the collided object
            if (player == null)
            {
                Debug.LogError("Player component not found on the collided object.");
                return;
            }
            if (player != null)
            {
                Destroy(gameObject); // Destroy the LifeUp object
                player.lives++; // Increment player lives
                player.livesText.text = "Lives: " + player.lives; // Update the UI text for lives
                Debug.Log("Player lives: " + player.lives); // Log the current lives
                if(player.lives >= 3)
                {
                    player.lives = 3; // Cap the lives at 3
                    Debug.Log("Lives capped at 3.");
                }
            }
        }
    }
}
