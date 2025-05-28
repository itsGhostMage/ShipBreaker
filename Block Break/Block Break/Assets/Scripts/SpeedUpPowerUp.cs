using UnityEngine;
using System.Collections;

public class SpeedUpPowerUp : MonoBehaviour
{
    public float hoverSpeed = 2f;
    public float lifeTime = 30f;
    public float speedIncrease = 5f;
    public float speedIncreaseDuration = 10f;

    private Ball targetBall;
    private float originalBallSpeed;
    private bool collected;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        transform.Rotate(-90, 0, 90);
        StartCoroutine(HoverRoutine());
        StartCoroutine(LifetimeRoutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if (other.CompareTag("Player")) // Player is the paddle
        {
            collected = true;
            Debug.Log("Speed power-up collected!");

            // Find the ball in the scene
            targetBall = FindFirstObjectByType<Ball>();
            if (targetBall != null)
            {
                originalBallSpeed = targetBall.speed;
                StartCoroutine(ApplySpeedBoost());
            }

            // Hide power-up immediately
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    IEnumerator ApplySpeedBoost()
    {
        // Apply speed boost
        targetBall.speed += speedIncrease;
        Debug.Log($"Ball speed increased to {targetBall.speed}");

        // Wait for duration
        yield return new WaitForSeconds(speedIncreaseDuration);

        // Revert speed
        targetBall.speed = originalBallSpeed;
        Debug.Log($"Ball speed reverted to {originalBallSpeed}");

        // Destroy power-up
        Destroy(gameObject);
    }

    IEnumerator HoverRoutine()
    {
        while (!collected)
        {
            float yOffset = Mathf.Sin(Time.time * hoverSpeed) * 0.5f;
            transform.position = startPosition + Vector3.up * yOffset;
            yield return null;
        }
    }

    IEnumerator LifetimeRoutine()
    {
        yield return new WaitForSeconds(lifeTime);
        if (!collected) Destroy(gameObject);
    }
}