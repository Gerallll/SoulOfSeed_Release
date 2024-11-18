using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupLatern : MonoBehaviour
{
    public GameObject intText;
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, gunContainer, fpsCam;
    public Light flashlight; // Referensi ke Point Light sebagai senter
    public AudioSource audioSource; // AudioSource untuk memutar SFX
    public AudioClip flashlightSFX; // SFX untuk suara on/off senter

    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public bool withinRange;
    public bool isPicked; // Menandai apakah objek sudah di-pick
    public bool wasPicked; // Menandakan apakah objek pernah di-pick

    // Static variable to ensure only one object can be picked up at a time
    public static bool isCarryingObject = false;

    private void Start()
    {
        intText.SetActive(false); // Pastikan intText dimulai dalam keadaan tidak aktif
        isPicked = false; // Inisialisasi objek belum diambil

        if (!equipped)
        {
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
        else
        {
            rb.isKinematic = true;
            coll.isTrigger = true;
        }

        if (flashlight != null)
        {
            flashlight.enabled = false; // Matikan senter di awal permainan
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Hanya aktifkan intText jika tidak sedang membawa object lain dan item ini belum diambil
        if (other.CompareTag("MainCamera") && !equipped && !isCarryingObject && !isPicked)
        {
            intText.SetActive(true);
            withinRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Sembunyikan intText saat pemain keluar dari collider
        if (other.CompareTag("MainCamera"))
        {
            intText.SetActive(false);
            withinRange = false;
        }
    }

    private void Update()
    {
        // Hanya ambil object jika pemain berada dalam range, tidak membawa object lain, dan menekan tombol E
        if (withinRange && Input.GetKeyDown(KeyCode.E) && !isCarryingObject && !isPicked)
        {
            PickUp();
        }

        // Cek apakah musuh sedang mengejar, jika iya, drop tidak bisa dilakukan
        if (equipped && Input.GetKeyDown(KeyCode.F) && !enemyAI1.isChasing)
        {
            Drop();
        }

        // Jika senter dipegang, hidupkan atau matikan dengan tombol C
        if (equipped && Input.GetKeyDown(KeyCode.C))
        {
            ToggleFlashlight();
        }

        // Jika object yang dipegang sudah tidak ada (misalnya dihancurkan), reset isCarryingObject
        if (equipped && gameObject == null)
        {
            isCarryingObject = false;
        }
    }

    private void OnDestroy()
    {
        // Pastikan saat objek dihancurkan, isCarryingObject di-reset
        if (equipped)
        {
            isCarryingObject = false;
        }
    }

    private void PickUp()
    {
        equipped = true;
        isCarryingObject = true; 
        isPicked = true; // Objek sudah di-pick
        wasPicked = true; // Tandai objek pernah di-pick

        // Menempatkan item di container
        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        // Nonaktifkan physics dan trigger collider
        rb.isKinematic = true;
        coll.isTrigger = true;

        intText.SetActive(false); // Sembunyikan intText setelah di-pickup
    }

    private void Drop()
    {
        equipped = false;
        isCarryingObject = false; // Bebaskan pemain untuk mengambil objek lain
        isPicked = false; // Reset isPicked agar bisa di-pick lagi

        // Lepaskan item dari pemain
        transform.SetParent(null);

        // Aktifkan kembali physics
        rb.isKinematic = false;
        coll.isTrigger = false;

        // Berikan velocity yang sama dengan player
        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        // Tambahkan gaya dorong ke arah depan dan atas saat drop
        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);

        // Tambahkan putaran acak
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);

        // Matikan senter saat di-drop
        if (flashlight != null)
        {
            flashlight.enabled = false;
        }
    }

    private void ToggleFlashlight()
    {
        if (flashlight != null)
        {
            flashlight.enabled = !flashlight.enabled; // Mengubah kondisi nyala/mati senter

            // Memutar SFX ketika senter dihidupkan/dimatikan
            if (audioSource != null && flashlightSFX != null)
            {
                audioSource.PlayOneShot(flashlightSFX);
            }
        }
    }

    public bool GetIsPicked() // Fungsi untuk mendapatkan status apakah objek sudah di-pick
    {
        return isPicked;
    }
}