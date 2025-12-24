using UnityEngine;
using UnityEngine.UI; // Untuk UI Bar
using System.Collections;
using TMPro; // Jika pakai TextMeshPro untuk Timer, kalau Text biasa ganti jadi 'Text'

public class FarmPlot : MonoBehaviour
{
    [Header("Settings")]
    public float growthTimePerStage = 5f; // Waktu tumbuh per stage (detik)

    [Header("References")]
    public GameObject carrotObject2D; // Object child yang ada SpriteRenderer-nya
    public SpriteRenderer carrotRenderer;
    public Sprite[] carrotStages; // Masukkan 3 sprite: Tier 1, Tier 2, Tier 3 (Siap Panen)

    [Header("UI References")]
    public GameObject uiCanvas; // World Space Canvas di atas tanah
    public Image progressBar;   // Image type Filled
    public TextMeshProUGUI timerText; // Text untuk timer

    // State
    private bool isPlanted = false;
    private bool isWatered = false;
    private bool isReadyToHarvest = false;
    private int currentTier = 0; // 0=Kosong, 1=Kecil, 2=Sedang, 3=Siap

    // Simulasi Player Inventory (Nanti diganti dengan script Inventory aslimu)
    // "SeedSack", "WateringCan", "Hand"
    public static string playerHeldItem = "Hand";

    void Start()
    {
        // Reset kondisi awal
        carrotObject2D.SetActive(false);
        uiCanvas.SetActive(false);
    }

    private void OnMouseDown()
    {
        // Logic Interaksi saat tanah/tanaman diklik
        if (!isPlanted)
        {
            // Cek apakah player pegang karung bibit
            if (playerHeldItem == "SeedSack")
            {
                PlantCarrot();
            }
            else
            {
                Debug.Log("Pegang Karung Bibit dulu buat nanam!");
            }
        }
        else if (isPlanted && !isWatered)
        {
            // Cek apakah player pegang penyiram
            if (playerHeldItem == "WateringCan")
            {
                WaterCarrot();
            }
            else
            {
                Debug.Log("Tanah kering! Pegang Penyiram buat nyiram.");
            }
        }
        else if (isReadyToHarvest)
        {
            HarvestCarrot();
        }
    }

    void PlantCarrot()
    {
        isPlanted = true;
        currentTier = 1; // Tier 1 (Kecil)

        carrotObject2D.SetActive(true);
        carrotRenderer.sprite = carrotStages[0]; // Set sprite Tier 1

        Debug.Log("Wortel ditanam! Butuh air.");
    }

    void WaterCarrot()
    {
        isWatered = true;
        StartCoroutine(GrowthRoutine());
    }

    IEnumerator GrowthRoutine()
    {
        uiCanvas.SetActive(true); // Munculin UI
        float timer = 0;

        // --- PROSES TUMBUH DARI TIER 1 KE 2 ---
        while (timer < growthTimePerStage)
        {
            timer += Time.deltaTime;
            UpdateUI(timer, growthTimePerStage);
            yield return null;
        }

        // Naik ke Tier 2
        currentTier = 2;
        carrotRenderer.sprite = carrotStages[1]; // Sprite Medium
        timer = 0; // Reset timer untuk phase selanjutnya

        // --- PROSES TUMBUH DARI TIER 2 KE 3 ---
        while (timer < growthTimePerStage)
        {
            timer += Time.deltaTime;
            UpdateUI(timer, growthTimePerStage);
            yield return null;
        }

        // Naik ke Tier 3 (Siap Panen)
        currentTier = 3;
        carrotRenderer.sprite = carrotStages[2]; // Sprite Siap Panen
        isReadyToHarvest = true;

        // Sembunyikan UI setelah selesai tumbuh
        uiCanvas.SetActive(false);
        Debug.Log("Wortel Siap Panen!");
    }

    void UpdateUI(float current, float max)
    {
        float progress = current / max;
        progressBar.fillAmount = progress;
        timerText.text = (max - current).ToString("F1") + "s";
    }

    void HarvestCarrot()
    {
        Debug.Log("Panen Berhasil! Dapat Wortel.");

        // Reset Tanah
        isPlanted = false;
        isWatered = false;
        isReadyToHarvest = false;
        currentTier = 0;

        carrotObject2D.SetActive(false);
        uiCanvas.SetActive(false);

        // Di sini bisa tambahkan logic nambah item ke inventory player
    }
}