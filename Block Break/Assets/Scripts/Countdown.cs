using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class Countdown : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI countdownText;
    [SerializeField] public float countdownDuration = 3f; // Duration of the countdown in seconds

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        countdownDuration -= Time.deltaTime; // Increment elapsed time by the time since the last frame
        countdownText.text =string.Format("{0:0}", countdownDuration); // Update the countdown text with the remaining time
        if (countdownDuration <= 0f) // Check if the countdown has reached zero
        {
            countdownText.gameObject.SetActive(false);
        }
    }
}
