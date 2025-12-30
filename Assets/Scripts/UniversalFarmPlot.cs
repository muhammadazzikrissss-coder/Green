using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UniversalFarmPlot : MonoBehaviour
{
    [Header("Visuals")]
    public SpriteRenderer plantRenderer;
    public GameObject uiCanvas;
    public Image progressBar; // <--- JANGAN LUPA DRAG INI DI INSPECTOR NANTI

    // State
    private CropData currentCrop;
    private bool isPlanted = false;
    private bool isWatered = false;
    private bool isReadyToHarvest = false;

    void Start()
    {
        // Sembunyikan visual awal
        plantRenderer.gameObject.SetActive(false);
        if (uiCanvas != null) uiCanvas.SetActive(false);
    }

    void OnMouseDown()
    {
        // 1. CEK FISIK: Apakah klik masuk?
        Debug.Log("--- KLIK DITERIMA TANAH ---");

        if (InventoryManager.Instance == null) return;
        ItemData heldItem = InventoryManager.Instance.GetCurrentItem();

        // 2. CEK ITEM: Apa yang dipegang?
        if (heldItem == null)
        {
            Debug.Log("Status: Tangan Kosong / Null");
            return;
        }

        Debug.Log($"Pegang Item: {heldItem.itemName} | Tipe: {heldItem.itemType}");

        // Khusus kalau Tool, kita intip Action Name-nya
        if (heldItem.itemType == ItemType.Tool)
        {
            // Tanda kutip ' ' biar kelihatan kalau ada spasi nyelip
            Debug.Log($"Action Name di Data: '{heldItem.toolActionName}'");
        }

        // --- MASUK LOGIC ---

        // A. MENANAM
        if (heldItem.itemType == ItemType.Seed && !isPlanted)
        {
            Plant(heldItem.cropData);
        }
        // B. MENYIRAM (Perhatikan Syaratnya!)
        else if (heldItem.itemType == ItemType.Tool && heldItem.toolActionName == "Watering")
        {
            // Cek apakah syarat terpenuhi?
            if (isPlanted == false)
            {
                Debug.Log("GAGAL SIRAM: Tanah masih kosong! Tanam dulu.");
            }
            else if (isWatered == true)
            {
                Debug.Log("GAGAL SIRAM: Tanah sudah basah!");
            }
            else
            {
                // Kalau lolos semua syarat
                Water();
            }
        }
        // C. PANEN
        else if (heldItem.itemType == ItemType.Tool && heldItem.toolActionName == "Hands")
        {
            if (isPlanted && isReadyToHarvest) Harvest();
        }
        else
        {
            Debug.Log("LOGIC LEWAT SEMUA: Tidak ada kondisi if yang cocok.");
        }
    }

    void Plant(CropData crop)
    {
        isPlanted = true;
        isWatered = false;
        isReadyToHarvest = false;

        currentCrop = crop;

        // Munculin Bibit (Tier 1)
        plantRenderer.sprite = currentCrop.growthSprites[0];
        plantRenderer.gameObject.SetActive(true);

        Debug.Log("Berhasil ditanam: " + crop.cropName + ". Silakan siram!");
    }

    void Water()
    {
        isWatered = true;
        Debug.Log("Tanaman disiram! Timer mulai berjalan...");

        // --- INI PERINTAH YANG TADI HILANG ---
        StartCoroutine(GrowthRoutine());
    }

    void Harvest()
    {
        Debug.Log("Panen BERHASIL! Dapat " + currentCrop.cropName);

        // Reset Tanah Jadi Kosong Lagi
        isPlanted = false;
        isWatered = false;
        isReadyToHarvest = false;
        currentCrop = null;

        plantRenderer.gameObject.SetActive(false);
        if (uiCanvas != null) uiCanvas.SetActive(false);
    }

    // --- MESIN WAKTU (COROUTINE) ---
    IEnumerator GrowthRoutine()
    {
        // 1. Munculkan UI Bar
        if (uiCanvas != null) uiCanvas.SetActive(true);

        float timeToGrow = currentCrop.growthTime; // Ambil waktu dari Data Tanaman
        float timer = 0;

        // --- FASE 1: Menuju Sedang (Tier 2) ---
        while (timer < timeToGrow)
        {
            timer += Time.deltaTime;
            // Update Bar (0% sampai 50%)
            if (progressBar != null) progressBar.fillAmount = (timer / timeToGrow) * 0.5f;
            yield return null;
        }

        // Ganti Gambar ke Tier 2 (Sedang)
        if (currentCrop.growthSprites.Length > 1)
            plantRenderer.sprite = currentCrop.growthSprites[1];

        // Reset timer buat fase berikutnya
        timer = 0;

        // --- FASE 2: Menuju Panen (Tier 3) ---
        while (timer < timeToGrow)
        {
            timer += Time.deltaTime;
            // Update Bar (50% sampai 100%)
            if (progressBar != null) progressBar.fillAmount = 0.5f + ((timer / timeToGrow) * 0.5f);
            yield return null;
        }

        // Ganti Gambar ke Tier 3 (Siap Panen)
        if (currentCrop.growthSprites.Length > 2)
            plantRenderer.sprite = currentCrop.growthSprites[2];

        // Selesai!
        isReadyToHarvest = true;
        if (uiCanvas != null) uiCanvas.SetActive(false); // Sembunyikan bar
        Debug.Log("Tanaman SIAP PANEN!");
    }
}