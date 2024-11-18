using System.Collections;
using UnityEngine;

public class DelayedAudioStart : MonoBehaviour
{
    public AudioSource audioSource; // Referensi ke AudioSource
    public CanvasGroup canvasGroup; // Referensi ke CanvasGroup untuk Canvas

    public float audioDelay = 5f; // Waktu delay dalam detik untuk audio
    public float audioFadeDuration = 2f; // Durasi fade-in dan fade-out untuk audio

    public float canvasFadeDuration = 2f; // Durasi fade-in dan fade-out untuk Canvas

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (canvasGroup != null)
        {
            // Mulai fade-in untuk Canvas
            StartCoroutine(FadeCanvas(canvasGroup, canvasFadeDuration, true));
        }

        // Mulai audio dengan delay dan fade-in
        StartCoroutine(DelayedPlayWithFadeIn());
    }

    private IEnumerator DelayedPlayWithFadeIn()
    {
        // Tunggu sesuai delay
        yield return new WaitForSeconds(audioDelay);

        // Mulai fade-in untuk AudioSource
        audioSource.Play();  // Pastikan audio mulai diputar
        StartCoroutine(FadeAudio(audioSource, audioFadeDuration, true));
    }

    // Fungsi untuk fade-in atau fade-out audio
    private IEnumerator FadeAudio(AudioSource source, float duration, bool fadeIn)
    {
        float startVolume = fadeIn ? 0f : source.volume;
        float targetVolume = fadeIn ? 1f : 0f;
        float timeElapsed = 0f;

        // Mulai fade audio
        while (timeElapsed < duration)
        {
            source.volume = Mathf.Lerp(startVolume, targetVolume, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Pastikan volume akhirnya mencapai target
        source.volume = targetVolume;

        if (!fadeIn)
        {
            source.Stop(); // Hentikan audio jika fade-out
        }
    }

    // Fungsi untuk fade-in atau fade-out canvas
    private IEnumerator FadeCanvas(CanvasGroup canvas, float duration, bool fadeIn)
    {
        float startAlpha = fadeIn ? 0f : canvas.alpha;
        float targetAlpha = fadeIn ? 1f : 0f;
        float timeElapsed = 0f;

        // Mulai fade canvas
        while (timeElapsed < duration)
        {
            canvas.alpha = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Pastikan alpha akhirnya mencapai target
        canvas.alpha = targetAlpha;
    }
}
