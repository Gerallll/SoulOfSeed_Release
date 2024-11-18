using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameOverManager gameOverManager; // Referensi untuk GameOverManager script
    public GameObject BGM;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player has entered the trigger
        if (other.CompareTag("Player"))
        {
            gameOverManager.TriggerGameOver();
            BGM.SetActive(false);
        }
    }
}
