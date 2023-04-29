using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scenepersist : MonoBehaviour
{
    void Awake()
    {
        int numScenePersists = FindObjectsOfType<scenepersist>().Length;

        // ensure that there is only 1 game session object at a time
        if (numScenePersists > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject); // doesn't destroy the object this script is attached to when a scene reloads
        }
    }

    public void ResetScenePersistence()
    {
        Destroy(gameObject);
    }
}
