using UnityEngine;
using UnityEngine.InputSystem;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class TouchControls : MonoBehaviour
{
    [SerializeField] private float speed = 30f;
    [SerializeField] private float xBound = 25f;
    private bool isTouching;
    private EnhancedTouch.Finger currentFinger;

    private void OnEnable()
    {
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
        EnhancedTouch.Touch.onFingerUp += FingerUp;
        EnhancedTouch.Touch.onFingerMove += FingerMove;
    }

    private void OnDisable()
    {
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
        EnhancedTouch.Touch.onFingerUp -= FingerUp;
        EnhancedTouch.Touch.onFingerMove -= FingerMove;
    }

    private void Update()
    {
        if (!isTouching) return;

        // Get the most recent touch position
        var touchPosition = currentFinger.currentTouch.screenPosition;

        // Calculate movement direction
        float direction = touchPosition.x < Screen.width / 2 ? -1f : 1f;

        // Move player
        transform.Translate(Vector3.right * (direction * speed * Time.deltaTime));

        // Clamp position
        var clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -xBound, xBound);
        transform.position = clampedPosition;
    }

    private void FingerDown(EnhancedTouch.Finger finger)
    {
        if (isTouching) return;

        isTouching = true;
        currentFinger = finger;
    }

    private void FingerUp(EnhancedTouch.Finger finger)
    {
        if (finger != currentFinger) return;

        isTouching = false;
        currentFinger = null;
    }

    private void FingerMove(EnhancedTouch.Finger finger)
    {
        if (finger != currentFinger) return;

        // Update logic is handled in Update using currentFinger
    }

    #if UNITY_EDITOR
    [SerializeField] private bool simulateTouch = true;
    #endif

    private void Start()
    {
        #if UNITY_EDITOR
        if (simulateTouch)
            EnhancedTouch.TouchSimulation.Enable();
          #endif
    }
}