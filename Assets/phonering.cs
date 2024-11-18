using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class phonering : MonoBehaviour
{
    public AudioSource phoneRingSound;  // Assign audio source with the ringing sound
    public MonoBehaviour gameObject1;
    public Collider gameObject2;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player has entered the trigger
        if (other.CompareTag("Player"))
        {
            // Play the ringing sound
            if (phoneRingSound != null && !phoneRingSound.isPlaying)
            {
                phoneRingSound.Play();
            }
            // Disable the trigger so it won't ring again
            GetComponent<Collider>().enabled = false;

            if (gameObject1 != null)
            {
                gameObject1.enabled = true;
            }

            if (gameObject2 != null)
            {
                gameObject2.enabled = true;
            }
        }
    }
}