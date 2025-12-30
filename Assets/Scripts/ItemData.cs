using UnityEngine;

public enum ItemType
{
    Tool,       // Alat (Penyiram, Pacul)
    Seed,       // Bibit (Butuh data tanaman)
    Consumable, // Barang habis pakai (Pupuk)
    None        // Tangan kosong
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("General Info")]
    public string itemName;
    public Sprite icon;
    public ItemType itemType;

    [Header("If Type is SEED")]
    public CropData cropData; // Masukkan data tanaman (script yang tadi) di sini kalau ini bibit

    [Header("If Type is TOOL")]
    public string toolActionName; // Misal: "Watering", "Harvesting"
}