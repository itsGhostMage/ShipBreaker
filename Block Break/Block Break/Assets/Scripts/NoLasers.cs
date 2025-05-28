using UnityEngine;
using System.Collections;

public class NoLasers : MonoBehaviour
{
    public float powerupDuration;
    public float rotationSpeed;
    private SpawnManager spawnManager;
    private bool powerupActive = false;

    void Start()
    {
        spawnManager = FindFirstObjectByType<SpawnManager>();
        if (spawnManager == null)
        {
            Debug.LogError("SpawnManager not found in scene!");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!powerupActive && other.CompareTag("Player"))
        {
            powerupActive = true;
            StartCoroutine(DisableLasersRoutine());
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    IEnumerator DisableLasersRoutine()
    {
        // Disable lasers on all current enemies
        SetLaserState(true);

        // Track new enemies during powerup duration
        float timer = 0f;
        while (timer < powerupDuration)
        {
            // Continuously disable lasers on newly spawned enemies
            SetLaserState(true);

            timer += Time.deltaTime;
            yield return null;
        }

        // Re-enable lasers on all active enemies
        SetLaserState(false);
        Destroy(gameObject);
    }

    void SetLaserState(bool state)
    {
        foreach (GameObject enemy in spawnManager.activeEnemies)
        {
            if (enemy != null)
            {
                Ship ship = enemy.GetComponent<Ship>();
                if (ship != null)
                {
                    ship.noLasersCollected = state;
                }
            }
        }
    }
}