using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Threading;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] public float currentTime = 60f;
    private bool isTimerRunning = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(3f); // Wait for 1 second before starting the timer
        StartCoroutine(TimerStart());
    }

    IEnumerator TimerStart()
    {
        isTimerRunning = true;
        while (currentTime > 0f && isTimerRunning)
        {
            currentTime -= Time.deltaTime; // Decrease the time by the time passed since the last frame
            timerText.text = Mathf.Round(currentTime).ToString(); // Update the timer text
            yield return null; // Wait for the next frame
        }

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            StartCoroutine(ReloadSceneCoroutine());
        }
        yield return null; // Wait for the next frame
    }

    public void AddTime(float timeToAdd)
    {
        currentTime += timeToAdd;
    }

    IEnumerator ReloadSceneCoroutine()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }
}