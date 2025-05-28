using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;
    private float height;
    private float speed;
    private float rotationSpeed;
    private float progress;
    private float totalDuration;
    private float totalRotation;

    public void Initialize(Vector3 startPos, Vector3 endPos, float height, float speed, float rotationSpeed)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        this.height = height;
        this.speed = speed;
        this.rotationSpeed = rotationSpeed;
        this.totalDuration = 1f / speed;
        this.totalRotation = rotationSpeed * totalDuration;
    }

    void Update()
    {
        if (progress < 1f)
        {
            progress += Time.deltaTime * speed;

            // Parabolic movement
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, progress);
            currentPos.y += Mathf.Sin(progress * Mathf.PI) * height;
            transform.position = currentPos;

            // Controlled rotation
            float currentRotation = Mathf.Repeat(totalRotation * progress, 180);
            transform.rotation = Quaternion.Euler(0, currentRotation, 0);
        }
        else
        {
            // Snap to final position and rotation
            transform.position = endPos;
            transform.rotation = Quaternion.Euler(0, Mathf.Round(totalRotation / 360) * 180, 0);
        }
    }
}