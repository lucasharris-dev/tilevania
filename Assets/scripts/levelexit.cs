using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // needed to access different scenes that have been added to File/Build Settings in Unity

public class levelexit : MonoBehaviour
{
    [SerializeField] float waitTime = 1f;
    
    IEnumerator NextLevel() // this delay method is called a coroutine
    {
        yield return new WaitForSecondsRealtime(waitTime); // to delay, this needs to be above what you want to execute, and the return type needs to be IEnumerator

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0; // reset so the game restarts if you finish, in a real game, could make the final scene a game over screen and let the user choose what to do
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(NextLevel()); // used to delay method execution
    }
}