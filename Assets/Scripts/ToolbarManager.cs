using UnityEngine;
using UnityEngine.UI; // Wajib ada buat ngatur UI Image/Button

public class ToolbarManager : MonoBehaviour
{
    [Header("Slot Backgrounds")]
    public Image[] slotImages; // Masukkan 3 Image background tombol di sini

    [Header("Colors")]
    public Color selectedColor = Color.green; // Warna pas dipilih
    public Color normalColor = Color.white;   // Warna pas ga dipilih

    void Start()
    {
        // Default pilih slot pertama (Tangan Kosong) pas game mulai
        SelectSlot(0);
    }

    // Fungsi ini dipanggil sama Button Unity nanti
    public void SelectItem(int slotIndex)
    {
        SelectSlot(slotIndex);

        // Logic ganti item global (sesuai script FarmPlot sebelumnya)
        switch (slotIndex)
        {
            case 0:
                FarmPlot.playerHeldItem = "Hand";
                Debug.Log("Item: Tangan Kosong");
                break;
            case 1:
                FarmPlot.playerHeldItem = "SeedSack";
                Debug.Log("Item: Karung Bibit");
                break;
            case 2:
                FarmPlot.playerHeldItem = "WateringCan";
                Debug.Log("Item: Penyiram Air");
                break;
        }
    }

    // Ini cuma buat visual ganti warna
    void SelectSlot(int index)
    {
        // Reset semua warna jadi putih dulu
        foreach (Image img in slotImages)
        {
            img.color = normalColor;
        }

        // Ubah warna yang dipilih jadi hijau
        if (index >= 0 && index < slotImages.Length)
        {
            slotImages[index].color = selectedColor;
        }
    }
}