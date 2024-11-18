using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogNormal : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // Text untuk dialog
    public GameObject intText; // GameObject untuk "Press E"
    public string[] lines; // Array untuk menampung dialog
    public float textSpeed; // Kecepatan teks
    private int index;
    public bool interactable = false; // Untuk mengecek apakah pemain ada di dalam area trigger
    private bool isDialogActive = false; // Untuk mengecek apakah dialog sedang aktif

    public MonoBehaviour SC_FPSController;
    public Pause pauseScript;
    public PauseApart pauseScript2;
    public AudioSource audioSource;
    public GameObject DialogBG;

    public AudioClip[] SpeakNoises = new AudioClip[0]; // Array suara gibberish
    public AudioSource gibberishSource; // AudioSource untuk gibberish

    public float gibberishPitch = 1.0f; // Kecepatan playback untuk audio gibberish (1.0 = normal speed)

    private Coroutine gibberishCoroutine; // Coroutine untuk gibberish
    private bool lineFinished = false; // Flag untuk mengecek apakah line sudah selesai
    private bool isGibberishPlaying = false; // Flag untuk mengecek apakah gibberish sedang diputar

    void Start()
    {
        textComponent.text = string.Empty;
        intText.SetActive(false); // Nonaktifkan teks "Press E" pada awalnya

        // Pengecekan awal untuk memastikan gibberishSource dan SpeakNoises diatur
        if (gibberishSource == null)
        {
            Debug.LogWarning("gibberishSource belum diatur. Pastikan AudioSource untuk gibberish di-assign.");
        }

        if (SpeakNoises == null || SpeakNoises.Length == 0)
        {
            Debug.LogWarning("SpeakNoises array kosong atau belum diatur. Pastikan setidaknya ada satu AudioClip.");
        }
    }

    void Update()
    {
        // Hanya memungkinkan interaksi jika pemain berada di area trigger dan dialog belum aktif
        if (interactable && !isDialogActive && Input.GetKeyDown(KeyCode.E))
        {
            StartDialog();
        }

        // Saat dialog aktif, pemain dapat menekan E untuk melanjutkan ke baris berikutnya
        if (isDialogActive && Input.GetKeyDown(KeyCode.E))
        {
            if (lineFinished)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index]; // Tampilkan teks lengkap
                StopGibberish(); // Stop audio gibberish
                lineFinished = true; // Tandai line sebagai selesai
            }
        }
    }

    void StartDialog()
    {
        index = 0;
        isDialogActive = true; // Tandai bahwa dialog aktif
        SC_FPSController.enabled = false; // Matikan script player movement
        audioSource.enabled = false;
        intText.SetActive(false);
        DialogBG.SetActive(true);

        if (pauseScript != null)
        {
            pauseScript.enabled = false;
        }

        if (pauseScript2 != null)
        {
            pauseScript2.enabled = false;
        }

        lineFinished = false; // Reset flag
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        lineFinished = false; // Reset flag
        textComponent.text = ""; // Kosongkan teks sebelumnya

        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;

            // Memutar suara gibberish secara acak dari array SpeakNoises
            PlayRandomGibberish();

            yield return new WaitForSeconds(textSpeed); // Tunggu sebelum karakter berikutnya muncul
        }

        lineFinished = true; // Tandai line sebagai selesai
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialog();
        }
    }

    void EndDialog()
    {
        isDialogActive = false;
        textComponent.text = string.Empty;
        SC_FPSController.enabled = true;
        audioSource.enabled = true;

        if (pauseScript != null)
        {
            pauseScript.enabled = true;
        }

        if (pauseScript2 != null)
        {
            pauseScript2.enabled = true;
        }

        DialogBG.SetActive(false);
    }

    void PlayRandomGibberish()
    {
        // Memeriksa apakah gibberishSource dan SpeakNoises valid sebelum memainkan audio
        if (SpeakNoises.Length > 0 && gibberishSource != null && !isGibberishPlaying)
        {
            AudioClip randomClip = SpeakNoises[Random.Range(0, SpeakNoises.Length)];
            gibberishSource.pitch = gibberishPitch;
            gibberishSource.PlayOneShot(randomClip);
            isGibberishPlaying = true;

            StartCoroutine(ResetGibberishPlaying(randomClip.length));
        }
    }

    private IEnumerator ResetGibberishPlaying(float duration)
    {
        yield return new WaitForSeconds(duration);
        isGibberishPlaying = false;
    }

    void StopGibberish()
    {
        if (gibberishSource != null)
        {
            gibberishSource.Stop();
        }
        isGibberishPlaying = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            if (!isDialogActive)
            {
                intText.SetActive(true);
                interactable = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            intText.SetActive(false);
            interactable = false;
        }
    }

    public void ResetDialog()
    {
        textComponent.text = string.Empty;
        intText.SetActive(false);
        interactable = false;
        isDialogActive = false;
        DialogBG.SetActive(false);
        StopGibberish();
    }
}
