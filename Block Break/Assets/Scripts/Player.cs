using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private float speed = 30f; // Speed of the player
    private float edgeLimit = 25f; // Limit for the player's movement
    [SerializeField] public int lives = 1; // Number of lives the player has
    [SerializeField] public TextMeshProUGUI livesText; // Reference to the TextMeshProUGUI component for displaying lives

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize the UI text (with null-check)
        if (livesText != null)
        {
            livesText.text = "" + lives;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Movement logic
        float horizontalInput = Input.GetAxis("Horizontal"); // Moved to Update
        transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);

        // Edge clamping
        float clampedX = Mathf.Clamp(transform.position.x, -edgeLimit, edgeLimit);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);

        // Scaling based on lives
        Vector3 newScale = lives switch
        {
            1 => new Vector3(0.5f, 1f, 1f),
            2 => new Vector3(0.75f, 1f, 1f),
            3 => new Vector3(1.0f, 1f, 1f),
            _ => transform.localScale
        };
        transform.localScale = newScale;

        if (lives <= 0)
        {
            livesText.text = " "; // Update the UI text for lives

            // Disable players mesh
            GetComponent<MeshRenderer>().enabled = false;

            StartCoroutine(ReloadSceneCoroutine());
        }
        else if (lives == 1)
        {
            livesText.text = "-"; // Update the UI text for lives
        }
        else if (lives == 2)
        {
            livesText.text = "- -"; // Update the UI text for lives
        }
        else if (lives >= 3)
        {
            lives = 3; // Cap the lives at 3
            livesText.text = "- - -"; // Update the UI text for lives
        }
    }

    IEnumerator ReloadSceneCoroutine()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }
}