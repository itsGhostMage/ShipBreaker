using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI hiScoreText;
    public int _currentScore = 0;
    private int _hiScore;
    public static Score Instance { get;  set; } // Singleton instance

    void Awake()
    {
       // PlayerPrefs.DeleteAll(); // Uncomment to reset PlayerPrefs for testing

        // Singleton pattern to ensure only one instance of Score exists
        if (Instance == null)
        {
            Instance = this;
            LoadHiScore(); // Load before scene changes
            SceneManager.sceneLoaded += OnSceneReload;
            DontDestroyOnLoad(gameObject); // Keep this object alive across scenes
        }
        else
        {
            Destroy(gameObject);
        }
        UpdateDisplays(); // Update displays on awake
    }

    void OnSceneReload(Scene scene, LoadSceneMode mode)
    {
        _currentScore = 0; // Reset score when a new scene is loaded

        // Find fresh UI references in the reloaded scene
        scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
        hiScoreText = GameObject.Find("HiScoreText")?.GetComponent<TextMeshProUGUI>();

        // Use a coroutine to wait for UI initialization
        UpdateDisplays();
    }

    void UpdateDisplays()
    {
        if (scoreText) scoreText.text = $"Score: {_currentScore}";
        if (hiScoreText) hiScoreText.text = $"Hi Score: {_hiScore}";
    }

    public void AddScore(int points)
    {
        _currentScore += points;
        UpdateScoreText();

        // Update high score immediately if surpassed
        if (_currentScore > _hiScore)
        {
            _hiScore = _currentScore;
            PlayerPrefs.SetInt("HiScore", _hiScore); // Sets the new HiScore to match the current score
            PlayerPrefs.Save(); // Save PlayerPrefs immediately
        }
        UpdateDisplays();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + _currentScore;
    }

    void LoadHiScore()
    {
        _hiScore = PlayerPrefs.GetInt("HiScore", 0);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneReload;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("HiScore", _hiScore); // Save the high score when the application quits
        PlayerPrefs.Save(); // Ensure PlayerPrefs are saved
    }
}