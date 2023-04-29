using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itempickup : MonoBehaviour
{
    [SerializeField] AudioClip itemPickupSound;
    [SerializeField] int itemValue = 100;

    bool wasCollected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !wasCollected)
        {
            wasCollected = true; // prevents picking up an item twice
            // need to do PlayClipAtPoint instead of Play because Play will stop immediately when Destroy is called, while PlayClipAtPoint will not because it instantiates the audio
            AudioSource.PlayClipAtPoint(itemPickupSound, Camera.main.transform.position); // use camera position because the camera is far away from the z position of the item,
            FindObjectOfType<gamesession>().AddToScore(itemValue);                          // so the sound will be quiet
            gameObject.SetActive(false); // an added layer of security just in case wasCollected doesn't work
            Destroy(gameObject);
        }
    }
}
