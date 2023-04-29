using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class gamesession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] float resetTime = 0.5f;
    [SerializeField] float restartTime = 1.5f;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    int startingPlayerLives;
    int score = 0;
    bool gameOver = false;

    // Awake is called when the component this script is on is first opened/instantiated, not when scenes load like Start (this is used for persistent data)
       // Awake is also called before Start
    void Awake()
    {
        int numGameSessions = FindObjectsOfType<gamesession>().Length;

        // ensure that there is only 1 game session object at a time
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject); // doesn't destroy the object this script is attached to when a scene reloads
        }

        startingPlayerLives = playerLives;
        score = 0;
    }

    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }

    public void AddToScore(int itemValue)
    {
        score += itemValue;
        scoreText.text = score.ToString();
    }

    public void ProcessPlayerDeath()
    {
        RemoveLife();
    }

    void RemoveLife()
    {
        playerLives--;

        if (playerLives == 0)
        {
            gameOver = true;
            livesText.text = playerLives.ToString();
            playerLives = startingPlayerLives;
            score = 0;
            StartCoroutine(ResetLevel(0, restartTime));
        }
        else
        {
            StartCoroutine(ResetLevel(SceneManager.GetActiveScene().buildIndex, resetTime));
        }
    }

    IEnumerator ResetLevel(int levelIndex, float resetTimeLength)
    {
        yield return new WaitForSecondsRealtime(resetTimeLength);

        SceneManager.LoadScene(levelIndex);
        
        if (gameOver)
        {
            FindObjectOfType<scenepersist>().ResetScenePersistence();
        }
        
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }
}
